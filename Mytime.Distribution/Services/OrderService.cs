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
            var order = await _orderRepository.Query()
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.CommissionHistory)
            .FirstOrDefaultAsync(e => e.OrderNo == request.OrderNo);
            if (order == null) return;

            var status = new OrderStatus[] { OrderStatus.PendingPayment, OrderStatus.PaymentFailed };
            if (!status.Contains(order.OrderStatus)) return;

            var roleResult = false;
            PartnerRole role = PartnerRole.Default;
            if (!string.IsNullOrEmpty(order.ExtendParams))
            {
                roleResult = Enum.TryParse<PartnerRole>(order.ExtendParams, true, out role);
            }

            var totalCommission = 0;
            var totalAmount = 0;
            var commissions = order.OrderItems.Where(e => e.CommissionHistory != null).Select(e => e.CommissionHistory);
            if (commissions.Count() > 0)
            {
                foreach (var item in commissions)
                {
                    if (order.IsFirstOrder)
                    {
                        totalAmount += item.Commission;
                        item.Status = CommissionStatus.Complete;
                        item.SettlementTime = DateTime.Now;
                    }
                    else
                    {
                        totalCommission += item.Commission;
                        item.Status = CommissionStatus.PendingSettlement;
                    }
                }
            }

            order.OrderStatus = OrderStatus.PaymentReceived;
            order.PaymentMethod = request.PaymentMethod;
            order.PaymentTime = request.PaymentTime;
            order.PaymentFee = request.PaymentFee;

            _orderRepository.Update(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                if (roleResult)
                {
                    await _customerRepository.UpdateProperyAsync(new Customer
                    {
                        Id = order.CustomerId,
                        Role = role
                    }, nameof(Customer.Role));
                }

                if (totalCommission > 0 || totalAmount > 0)
                {
                    var parentId = commissions.Select(e => e.CustomerId).FirstOrDefault();
                    await _customerManager.UpdateAssets(parentId, totalCommission, totalAmount, "下级首单返佣金");
                }

                if (order.WalletAmount > 0)
                {
                    await _customerManager.UpdateAssets(order.CustomerId, 0, 0, -order.WalletAmount, "商品抵扣金额");
                }

                transaction.Commit();
            }
        }

        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<Result> Cancel(int orderId, string reason)
        {
            var order = await _orderRepository.Query().FirstOrDefaultAsync(e => e.Id == orderId);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            if (order.OrderStatus == OrderStatus.Canceled)
                return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许删除");

            var status = new OrderStatus[] { OrderStatus.PaymentFailed, OrderStatus.PendingPayment };
            if (!status.Contains(order.OrderStatus))
                return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许删除");

            order.OrderStatus = OrderStatus.Canceled;
            order.CancelReason = reason;
            order.CancelTime = DateTime.Now;
            _orderRepository.Update(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                transaction.Commit();
            }

            return Result.Ok();
        }
    }
}