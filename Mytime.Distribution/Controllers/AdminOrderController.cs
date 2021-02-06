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
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;
using Mytime.Distribution.Utils.Helpers;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台订单管理
    /// </summary>
    [Authorize(Roles = "Admin,Accounting")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/order")]
    [Produces("application/json")]
    public class AdminOrderController : ControllerBase
    {
        private readonly IRepositoryByInt<Orders> _orderRepository;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IRepositoryByInt<PartnerApply> _partnerApplyRepository;
        private readonly IRepositoryByInt<Goods> _goodsRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="partnerApplyRepository"></param>
        /// <param name="goodsRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public AdminOrderController(IRepositoryByInt<Orders> orderRepository,
                                    IRepositoryByInt<Customer> customerRepository,
                                    IRepositoryByInt<PartnerApply> partnerApplyRepository,
                                    IRepositoryByInt<Goods> goodsRepository,
                                    ICustomerManager customerManager,
                                    IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _partnerApplyRepository = partnerApplyRepository;
            _goodsRepository = goodsRepository;
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
            if (!string.IsNullOrEmpty(request.OrderNo))
            {
                queryable = queryable.Where(e => e.OrderNo == request.OrderNo);
            }
            if (request.Status.HasValue)
            {
                queryable = queryable.Where(e => e.OrderStatus == request.Status.Value);
            }

            var totalRows = await queryable.CountAsync();
            var orders = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .Select(e => new AdminOrderListResponse
            {
                Id = e.Id,
                AvatarUrl = e.Customer.AvatarUrl,
                NickName = e.Customer.NickName,
                Createat = e.Createat,
                OrderNo = e.OrderNo,
                OrderStatus = e.OrderStatus,
                PaymentFee = e.PaymentFee,
                PaymentMethod = e.PaymentMethod,
                PaymentTime = e.PaymentTime,
                PaymentType = e.PaymentType,
                TotalFee = e.TotalFee,
                ActuallyAmount = e.ActuallyAmount,
                WalletAmount = e.WalletAmount
            })
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, orders));
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
        /// 添加订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create(AdminOrderCreateRequest request)
        {
            var customer = await _customerRepository.Query()
            .Include(e => e.Parent)
            .FirstOrDefaultAsync(e => e.Id == request.CustomerId);
            if (customer == null) return Result.Fail(ResultCodes.IdInvalid);
            if (customer.Role == PartnerRole.Default && request.PartnerRole == PartnerRole.Default) return Result.Fail(ResultCodes.RequestParamError, "请选择合伙人角色");
            if (customer.Role != PartnerRole.Default && request.Goods.Count == 0) return Result.Fail(ResultCodes.RequestParamError, "请选择商品");

            var now = DateTime.Now;
            var isFirstOrder = false;
            var totalFee = 0;
            var actuallyAmount = 0;
            var extendParams = string.Empty;
            var totalCommission = 0;
            var commissionRatio = 0f;
            var orderItems = new List<OrderItem>();
            var parentUser = customer.Parent;

            if (request.PartnerRole != PartnerRole.Default)
            {
                if (customer.Role != PartnerRole.Default) return Result.Fail(ResultCodes.RequestParamError, $"用户：{customer.NickName} 为{customer.Role.GetDescription()} 不能重复设置角色");
                var partner = await _partnerApplyRepository.Query()
                .Include(e => e.PartnerApplyGoods).ThenInclude(e => e.Goods).ThenInclude(e => e.ThumbnailImage)
                .Where(e => e.PartnerRole == request.PartnerRole)
                .FirstOrDefaultAsync();
                if (partner == null) return Result.Fail(ResultCodes.RequestParamError, $"角色{request.PartnerRole.GetDescription()}未设置商品");
                if (partner.PartnerApplyGoods.Count == 0) return Result.Fail(ResultCodes.RequestParamError, "没有商品");

                if (parentUser != null)
                {
                    if (parentUser.Role == partner.PartnerRole)
                    {
                        if (partner.ReferralCommissionRatio > 0)
                        {
                            commissionRatio = partner.ReferralCommissionRatio / 100f;
                        }
                    }
                    else
                    {
                        var ratio = await _partnerApplyRepository.Query()
                        .Where(e => e.PartnerRole == parentUser.Role)
                        .Select(e => e.RepurchaseCommissionRatio)
                        .FirstOrDefaultAsync();
                        if (ratio > 0)
                        {
                            commissionRatio = ratio / 100f;
                        }
                    }
                }

                isFirstOrder = true;
                customer.Role = request.PartnerRole;
                extendParams = Enum.GetName(typeof(PartnerRole), request.PartnerRole);
                totalFee = partner.OriginalPrice == 0 ? partner.TotalAmount : partner.OriginalPrice;
                actuallyAmount = partner.TotalAmount;
                var goodsQuantity = partner.PartnerApplyGoods.Sum(e => e.Quantity);
                var averagePrice = partner.TotalAmount / goodsQuantity;
                var quantity = 0;
                foreach (var item in partner.PartnerApplyGoods)
                {
                    quantity += item.Quantity;
                    if (quantity == goodsQuantity)
                    {
                        averagePrice = partner.TotalAmount - ((quantity - 1) * averagePrice);
                    }
                    var commission = (int)(averagePrice * commissionRatio);
                    totalCommission = item.Quantity * commission;
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        var orderItem = new OrderItem
                        {
                            GoodsId = item.GoodsId,
                            GoodsName = item.Goods.Name,
                            GoodsPrice = item.Goods.Price,
                            DiscountAmount = item.Goods.Price,
                            GoodsMediaUrl = item.Goods.ThumbnailImage?.Url,
                            Quantity = 1,
                            Remarks = string.Empty,
                            Status = OrderItemStatus.Default,
                            Createat = now,
                            IsFirstBatchGoods = true
                        };

                        orderItem.SetCommissionHistory(customer.ParentId, commission, (int)(commissionRatio * 100), CommissionStatus.Complete);

                        orderItems.Add(orderItem);
                    }
                }
            }
            else if (request.Goods.Count > 0)
            {
                if (customer.Role == PartnerRole.Default) return Result.Fail(ResultCodes.RequestParamError, $"用户：{customer.NickName} 为未设置合伙人角色");
                if (parentUser != null)
                {
                    var ratio = await _partnerApplyRepository.Query()
                    .Where(e => e.PartnerRole == parentUser.Role)
                    .Select(e => e.RepurchaseCommissionRatio)
                    .FirstOrDefaultAsync();
                    if (ratio > 0)
                    {
                        commissionRatio = ratio / 100f;
                    }
                }
                var goodsIds = request.Goods.Select(e => e.Id).Distinct();
                var goodses = await _goodsRepository.Query()
                .Include(e => e.ThumbnailImage)
                .Where(e => goodsIds.Contains(e.Id))
                .ToListAsync();
                foreach (var item in goodses)
                {
                    var first = request.Goods.FirstOrDefault(e => e.Id == item.Id);
                    if (first == null) return Result.Fail(ResultCodes.RequestParamError, $"产品{item.Name}不存在");
                    if (!item.IsPublished) return Result.Fail(ResultCodes.RequestParamError, $"产品{item.Name}未发布");

                    var discountRate = 100f;
                    if (customer.Role == PartnerRole.CityPartner && item.CityDiscount > 0)
                        discountRate = item.CityDiscount;
                    else if (customer.Role == PartnerRole.BranchPartner && item.BranchDiscount > 0)
                        discountRate = item.BranchDiscount;

                    var amount = (int)(item.Price * (discountRate / 100f));
                    var commission = (int)(amount * commissionRatio);
                    totalCommission += first.Quantity * commission;

                    for (int i = 0; i < first.Quantity; i++)
                    {
                        var orderItem = new OrderItem
                        {
                            GoodsId = item.Id,
                            GoodsName = item.Name,
                            GoodsPrice = item.Price,
                            DiscountAmount = amount, //折扣
                            GoodsMediaUrl = item.ThumbnailImage?.Url,
                            Quantity = 1,
                            Remarks = first.Remarks,
                            Status = OrderItemStatus.Default,
                            Createat = now,
                        };

                        orderItem.SetCommissionHistory(customer.ParentId, commission, (int)(commissionRatio * 100), CommissionStatus.PendingSettlement);

                        orderItems.Add(orderItem);
                    }
                }
                totalFee = orderItems.Sum(e => e.Quantity * e.GoodsPrice);
                actuallyAmount = orderItems.Sum(e => e.Quantity * e.DiscountAmount);
            }

            var order = new Orders(GenerateHelper.GenOrderNo())
            {
                CustomerId = customer.Id,
                OrderStatus = OrderStatus.PaymentReceived,
                PaymentType = request.PaymentType,
                PaymentMethod = request.PaymentMethod,
                PaymentFee = actuallyAmount,
                PaymentTime = now,
                Remarks = request.Remarks,
                Createat = now,
                TotalFee = totalFee,
                ActuallyAmount = actuallyAmount,
                ExtendParams = extendParams,
                IsFirstOrder = isFirstOrder,
                OrderItems = orderItems
            };

            _orderRepository.Insert(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                if (parentUser != null && totalCommission > 0)
                {
                    if (isFirstOrder)
                    {
                        await _customerManager.UpdateAssets(parentUser.Id, 0, totalCommission, "下级首单返佣金");
                    }
                    else
                    {
                        await _customerManager.UpdateCommission(parentUser.Id, totalCommission);
                    }
                }

                if (isFirstOrder)
                {
                    await _customerRepository.UpdateAsync(customer);
                }

                transaction.Commit();
            }

            return Result.Ok();
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
                        item.Status = CommissionStatus.PendingSettlement;
                        totalCommission += item.Commission;
                    }
                }
            }

            order.PaymentType = request.PaymentType;
            order.PaymentMethod = request.PaymentMethod;
            order.PaymentTime = DateTime.Now;
            order.PaymentFee = order.ActuallyAmount;
            order.OrderStatus = OrderStatus.PaymentReceived;

            _orderRepository.Update(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                if (totalCommission > 0 || totalAmount > 0)
                {
                    var parentId = commissions.Select(e => e.CustomerId).FirstOrDefault();
                    await _customerManager.UpdateAssets(parentId, totalCommission, totalAmount, "下级首单返佣金");
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
            var order = await _orderRepository.Query().FirstOrDefaultAsync(e => e.Id == id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);
            if (order.OrderStatus != OrderStatus.PendingPayment) return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许修改");

            order.OrderStatus = OrderStatus.Canceled;
            order.CancelReason = "手动取消";
            order.CancelTime = DateTime.Now;

            _orderRepository.Update(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

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