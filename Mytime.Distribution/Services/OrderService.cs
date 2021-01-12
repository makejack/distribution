using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;
using Mytime.Distribution.Services.Models.Request;
using Mytime.Distribution.Services.Models.Response;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IRepositoryByInt<Orders> _orderRepository;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly ICustomerManager _customerManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="customerManager"></param>
        public OrderService(IRepositoryByInt<Orders> orderRepository,
                            IRepositoryByInt<Customer> customerRepository,
                            ICustomerManager customerManager)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _customerManager = customerManager;
        }

        /// <summary>
        /// 支付接收
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task PaymentReceived(PaymentReceivedRequest request)
        {
            var order = await _orderRepository.Query().FirstOrDefaultAsync(e => e.OrderNo == request.OrderNo);
            if (order == null) return;

            var status = new OrderStatus[] { OrderStatus.PendingPayment, OrderStatus.PaymentFailed };
            if (!status.Contains(order.OrderStatus)) return;

            order.OrderStatus = OrderStatus.PaymentReceived;
            order.PaymentMethod = request.PaymentMethod;
            order.PaymentTime = request.PaymentTime;
            order.PaymentFee = request.PaymentFee;

            await _orderRepository.UpdateProperyAsync(order);

            if (string.IsNullOrEmpty(order.ExtendParams)) return;

            var result = Enum.TryParse<PartnerRole>(order.ExtendParams, true, out var role);
            if (!result) return;

            var customerRole = await _customerRepository.Query().Where(e => e.Id == order.CustomerId).Select(e => e.Role).FirstOrDefaultAsync();
            if (customerRole != PartnerRole.Default) return;

            // customer.Role = role;
            await _customerRepository.UpdateProperyAsync(new Customer
            {
                Id = order.CustomerId,
                Role = role
            }, nameof(Customer.Role));

        }

        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<Result> Cancel(int orderId, string reason)
        {
            var order = await _orderRepository.Query()
            .Include(e => e.OrderItems).ThenInclude(e => e.CommissionHistory)
            .FirstOrDefaultAsync(e => e.Id == orderId);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            if (order.OrderStatus == OrderStatus.Canceled)
                return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许删除");

            var status = new OrderStatus[] { OrderStatus.PaymentFailed, OrderStatus.PendingPayment };
            if (!status.Contains(order.OrderStatus))
                return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许删除");

            order.OrderStatus = OrderStatus.Canceled;
            order.CancelReason = reason;
            order.CancelTime = DateTime.Now;

            int? customerId = null;
            var refundCommission = 0;
            var commissions = order.OrderItems.Select(e => e.CommissionHistory);
            var commission = commissions.FirstOrDefault();
            if (commission != null)
            {
                customerId = commission.CustomerId;
                refundCommission = commissions.Sum(e => e.Commission);
                foreach (var item in commissions)
                {
                    item.Status = CommissionStatus.Invalidation;
                }
            }

            _orderRepository.Update(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                if (customerId.HasValue)
                {
                    await _customerManager.UpdateAssets(customerId.Value, -refundCommission, 0);
                }

                transaction.Commit();
            }

            return Result.Ok();
        }
    }
}