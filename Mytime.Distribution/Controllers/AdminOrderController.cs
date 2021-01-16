using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台订单管理
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/order")]
    [Produces("application/json")]
    public class AdminOrderController : ControllerBase
    {
        private readonly IRepositoryByInt<Orders> _orderRepository;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public AdminOrderController(IRepositoryByInt<Orders> orderRepository,
        IRepositoryByInt<Customer> customerRepository,
                                    ICustomerManager customerManager,
                                    IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _customerManager = customerManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminOrderListRequest request)
        {
            var queryable = _orderRepository.Query();

            if (request.CustomerId.HasValue)
            {
                queryable = queryable.Where(e => e.CustomerId == request.CustomerId.Value);
            }
            if (request.Status.HasValue)
            {
                queryable = queryable.Where(e => e.OrderStatus == request.Status.Value);
            }

            var totalRows = await queryable.CountAsync();
            var orders = await queryable.OrderByDescending(e => e.Id).Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminOrderListResponse>>(orders)));
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var order = await _orderRepository.Query()
            .Include(e => e.OrderItems)
            .Include(e => e.OrderBilling)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminOrderGetResponse>(order));
        }

        /// <summary>
        /// 编辑订单信息
        /// /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit([FromBody] AdminOrderEditRequest request)
        {
            var order = await _orderRepository.Query()
            .Include(e => e.OrderItems).ThenInclude(e => e.CommissionHistory)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);
            var status = new[] { OrderStatus.Closed, OrderStatus.PaymentReceived, OrderStatus.Complete };
            if (status.Contains(order.OrderStatus)) return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许修改");

            var totalCommission = 0;
            var commissions = order.OrderItems.Where(e => e.CommissionHistory != null).Select(e => e.CommissionHistory);
            if (commissions.Count() > 0)
            {
                foreach (var item in commissions)
                {
                    if (item.Status != CommissionStatus.Invalidation) continue;

                    item.Status = CommissionStatus.PendingSettlement;
                    totalCommission += item.Commission;
                }
            }

            order.PaymentType = request.PaymentType;
            order.PaymentMethod = request.PaymentMethod;
            order.PaymentTime = DateTime.Now;
            order.PaymentFee = order.TotalWithDiscount;

            _orderRepository.Update(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                if (totalCommission > 0)
                {
                    var parentId = commissions.Select(e => e.CustomerId).FirstOrDefault();
                    await _customerManager.UpdateAssets(parentId, totalCommission, 0);
                }

                if (!string.IsNullOrEmpty(order.ExtendParams))
                {
                    var result = Enum.TryParse<PartnerRole>(order.ExtendParams, true, out var role);
                    if (result)
                    {
                        await _customerRepository.UpdateProperyAsync(new Customer
                        {
                            Id = order.CustomerId,
                            Role = role
                        }, nameof(Customer.Role));
                    }
                }

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 订单取消
        /// </summary>
        /// <returns></returns>
        [HttpPut("cancel/{id}")]
        public async Task<Result> Cancel(int id)
        {
            var order = await _orderRepository.Query()
            .Include(e => e.OrderItems).ThenInclude(e => e.CommissionHistory)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);
            if (order.OrderStatus != OrderStatus.PendingPayment) return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许修改");

            var totalCommision = 0;
            var commissions = order.OrderItems.Where(e => e.CommissionHistory != null).Select(e => e.CommissionHistory);
            if (commissions.Count() > 0)
            {
                foreach (var item in commissions)
                {
                    item.Status = CommissionStatus.Invalidation;
                    totalCommision += item.Commission;
                }
            }

            order.OrderStatus = OrderStatus.Canceled;
            order.CancelReason = "手动取消";
            order.CancelTime = DateTime.Now;

            _orderRepository.Update(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                if (totalCommision > 0)
                {
                    var parentId = commissions.Select(e => e.CustomerId).FirstOrDefault();
                    await _customerManager.UpdateAssets(parentId, -totalCommision, 0);
                }

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            await _orderRepository.RemoveAsync(order);

            return Result.Ok();
        }
    }
}