using System.Collections.Generic;
using System;
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
using Mytime.Distribution.Utils.Helpers;
using Mytime.Distribution.Services;
using Mytime.Distribution.Services.Models.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Configs;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mytime.Distribution.Events;
using Mytime.Distribution.Utils.Constants;
using Mytime.Distribution.Services.SmsContent;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/order")]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAdminUserManager _adminUserManager;
        private readonly IRepositoryByInt<Orders> _orderRepository;
        private readonly IRepositoryByInt<OrderBilling> _orderBillingRepository;
        private readonly IRepositoryByInt<Goods> _goodsRepository;
        private readonly IRepositoryByInt<Assets> _assetsRepository;
        private readonly IRepositoryByInt<PartnerApply> _partnerApplyRepository;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly ICustomerManager _customerManager;
        private readonly IRabbitMQClient _client;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="orderRepository"></param>
        /// <param name="adminUserManager"></param>
        /// <param name="orderBillingRepository"></param>
        /// <param name="goodsRepository"></param>
        /// <param name="assetsRepository"></param>
        /// <param name="partnerApplyRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="paymentService"></param>
        /// <param name="orderService"></param>
        /// <param name="customerManager"></param>
        /// <param name="client"></param>
        /// <param name="cache"></param>
        /// <param name="mapper"></param>
        public OrderController(ILogger<OrderController> logger,
                               IAdminUserManager adminUserManager,
                               IRepositoryByInt<Orders> orderRepository,
                               IRepositoryByInt<OrderBilling> orderBillingRepository,
                               IRepositoryByInt<Goods> goodsRepository,
                               IRepositoryByInt<Assets> assetsRepository,
                               IRepositoryByInt<PartnerApply> partnerApplyRepository,
                               IRepositoryByInt<Customer> customerRepository,
                               IPaymentService paymentService,
                               IOrderService orderService,
                               ICustomerManager customerManager,
                               IRabbitMQClient client,
                               IMemoryCache cache,
                               IMapper mapper)
        {
            _logger = logger;
            _adminUserManager = adminUserManager;
            _orderRepository = orderRepository;
            _orderBillingRepository = orderBillingRepository;
            _goodsRepository = goodsRepository;
            _assetsRepository = assetsRepository;
            _partnerApplyRepository = partnerApplyRepository;
            _customerRepository = customerRepository;
            _paymentService = paymentService;
            _orderService = orderService;
            _customerManager = customerManager;
            _client = client;
            _cache = cache;
            _mapper = mapper;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrderCreateResponse), 200)]
        public async Task<Result> Create([FromBody] OrderCreateRequest request)
        {
            if (request == null) return Result.Fail(ResultCodes.RequestParamError);
            if (request.Items == null || request.Items.Count <= 0) return Result.Fail(ResultCodes.RequestParamError, "请选择商品");
            if (request.Items.Any(c => c.Quantity <= 0)) return Result.Fail(ResultCodes.RequestParamError, "购买商品数量必须大于0");

            DateTime? timeNull = null;
            Assets assets = null;
            var user = await _customerManager.GetUserAndParentAsync();

            var commissionRatio = 0f;//佣金百分比
            var totalCommission = 0;//总佣金
            var extendParams = string.Empty;
            if (user.Role != PartnerRole.Default)
            {
                var parentUser = user.Parent;
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
            }

            var goodsIds = request.Items.Select(c => c.Id).Distinct();
            var goods = await _goodsRepository.Query()
            .Include(e => e.ThumbnailImage)
            .Where(e => goodsIds.Contains(e.Id)).ToListAsync();

            if (goods.Count <= 0) return Result.Fail(ResultCodes.RequestParamError, "商品不存在");

            var orderItems = new List<OrderItem>();
            foreach (var item in goods)
            {
                var first = request.Items.FirstOrDefault(e => e.Id == item.Id);
                if (first == null) return Result.Fail(ResultCodes.RequestParamError, $"产品{item.Name}不存在");
                if (!item.IsPublished) return Result.Fail(ResultCodes.RequestParamError, $"产品{item.Name}未发布");

                var discountRate = 100f;
                if (user.Role == PartnerRole.CityPartner && item.CityDiscount > 0)
                    discountRate = item.CityDiscount;
                else if (user.Role == PartnerRole.BranchPartner && item.BranchDiscount > 0)
                    discountRate = item.BranchDiscount;

                var amount = (int)(item.Price * (discountRate / 100f));
                var commission = (int)(amount * commissionRatio);

                for (int i = 0; i < first.Quantity; i++)
                {
                    totalCommission += commission;

                    //创建订单时不选择商品选项，发货时在选择商品选项
                    var orderItem = new OrderItem
                    {
                        GoodsId = item.Id,
                        GoodsName = item.Name,
                        GoodsPrice = item.Price,
                        DiscountAmount = amount, //折扣
                        GoodsMediaUrl = item.ThumbnailImage?.Url,
                        Quantity = 1,
                        Remarks = first.Remarks,
                        ShippingStatus = ShippingStatus.Default,
                        Createat = DateTime.Now,
                    };
                    if (user.ParentId.HasValue && commission > 0)
                    {
                        orderItem.CommissionHistory = new CommissionHistory
                        {
                            CustomerId = user.ParentId.Value,
                            Commission = commission,
                            Percentage = (int)(commissionRatio * 100),
                            Status = CommissionStatus.Invalidation,
                            Createat = DateTime.Now,
                        };
                    }
                    orderItems.Add(orderItem);
                }
            }

            var totalFee = orderItems.Sum(e => e.Quantity * e.GoodsPrice);
            var actuallyAmount = orderItems.Sum(e => e.Quantity * e.DiscountAmount);
            var walletAmount = 0; //钱包扣除金额
            if (request.IsUseWallet)
            {
                assets = await _assetsRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == user.Id);
                if (assets.AvailableAmount > 0)
                {
                    if (assets.AvailableAmount > actuallyAmount)
                    {
                        walletAmount = assets.AvailableAmount - actuallyAmount;
                    }
                    else
                    {
                        walletAmount = actuallyAmount - assets.AvailableAmount;
                    }
                    double ratio = ((double)walletAmount / actuallyAmount);
                    actuallyAmount -= walletAmount;
                    foreach (var item in orderItems)
                    {
                        item.DiscountAmount -= (int)(item.DiscountAmount * ratio);
                        if (actuallyAmount == 0)
                        {
                            item.CommissionHistory.Status = CommissionStatus.PendingSettlement;
                        }
                    }
                }
            }

            var order = new Orders(GenerateHelper.GenOrderNo())
            {
                CustomerId = user.Id,
                OrderStatus = actuallyAmount > 0 ? OrderStatus.PendingPayment : OrderStatus.PaymentReceived,
                PaymentType = request.PaymentType,
                PaymentMethod = actuallyAmount == 0 ? PaymentMethods.Wallet : PaymentMethods.Wechat,
                Remarks = request.Remarks,
                Createat = DateTime.Now,
                TotalFee = totalFee,
                ActuallyAmount = actuallyAmount,
                WalletAmount = walletAmount,
                ExtendParams = extendParams,
                PaymentTime = actuallyAmount == 0 ? DateTime.Now : timeNull,
                OrderItems = orderItems,
            };

            _orderRepository.Insert(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                if (user.ParentId.HasValue && actuallyAmount == 0 && totalCommission > 0)
                {
                    await _customerManager.UpdateAssets(user.ParentId.Value, totalCommission);
                }

                if (actuallyAmount == 0 && walletAmount > 0)
                {
                    await _customerManager.UpdateAssets(assets, -walletAmount, "商品抵扣");
                }

                transaction.Commit();
            }
            return Result.Ok(_mapper.Map<OrderCreateResponse>(order));
        }

        /// <summary>
        /// 创建合伙人订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("partner")]
        public async Task<Result> Partner(OrderPartnerRequest request)
        {
            if (request.Role == PartnerRole.Default) return Result.Fail(ResultCodes.RequestParamError, "请选择合伙人角色");
            var user = await _customerManager.GetUserAndParentAsync();
            if (user.Role != PartnerRole.Default) return Result.Fail(ResultCodes.RequestParamError, "当前用户已经是合伙人");

            var partnerApply = await _partnerApplyRepository.Query()
            .Include(e => e.PartnerApplyGoods).ThenInclude(e => e.Goods).ThenInclude(e => e.ThumbnailImage)
            .Where(e => e.PartnerRole == request.Role)
            .FirstOrDefaultAsync();
            if (partnerApply == null) return Result.Fail(ResultCodes.RequestParamError);
            if (partnerApply.PartnerApplyGoods.Count == 0) return Result.Fail(ResultCodes.RequestParamError, "没有商品");

            var totalCommission = 0;
            var commissionRatio = 0f;
            if (user.Parent != null)
            {
                var parentUser = user.Parent;
                if (parentUser.Role == partnerApply.PartnerRole)
                {
                    if (partnerApply.ReferralCommissionRatio > 0)
                    {
                        commissionRatio = partnerApply.ReferralCommissionRatio / 100f;
                    }
                }
                else
                {
                    var ratio = await _partnerApplyRepository.Query()
                    .Where(e => e.PartnerRole == parentUser.Role)
                    .Select(e => e.ReferralCommissionRatio)
                    .FirstOrDefaultAsync();
                    if (ratio > 0)
                    {
                        commissionRatio = ratio / 100f;
                    }
                }
            }
            var extendParams = Enum.GetName(typeof(PartnerRole), request.Role);

            // var applyGoodses = partnerApply.PartnerApplyGoods;
            var goodsQuantity = partnerApply.PartnerApplyGoods.Sum(e => e.Quantity);
            var averagePrice = partnerApply.TotalAmount / goodsQuantity;
            var remainder = partnerApply.TotalAmount % goodsQuantity;
            var quantity = 0;
            var orderItems = new List<OrderItem>();
            foreach (var item in partnerApply.PartnerApplyGoods)
            {
                // var goods = applyGoodses.FirstOrDefault(e => e.GoodsId == item.Id);
                quantity += item.Quantity;
                if (quantity == goodsQuantity)
                {
                    averagePrice += remainder;
                }
                var commission = (int)(averagePrice * commissionRatio);
                for (int i = 0; i < item.Quantity; i++)
                {
                    totalCommission += commission;

                    var orderItem = new OrderItem
                    {
                        GoodsId = item.GoodsId,
                        GoodsName = item.Goods.Name,
                        GoodsPrice = item.Goods.Price,
                        DiscountAmount = item.Goods.Price, //折扣
                        GoodsMediaUrl = item.Goods.ThumbnailImage?.Url,
                        Quantity = 1,
                        Remarks = string.Empty,
                        ShippingStatus = ShippingStatus.Default,
                        Createat = DateTime.Now,
                        IsFirstBatchGoods = true
                    };

                    if (user.ParentId.HasValue && commission > 0)
                    {
                        orderItem.CommissionHistory = new CommissionHistory
                        {
                            CustomerId = user.ParentId.Value,
                            Commission = commission,
                            Percentage = (int)(commissionRatio * 100),
                            Status = CommissionStatus.Invalidation,
                            Createat = DateTime.Now,
                        };
                    }

                    orderItems.Add(orderItem);
                }
            }

            var order = new Orders(GenerateHelper.GenOrderNo())
            {
                CustomerId = user.Id,
                OrderStatus = OrderStatus.PendingPayment,
                PaymentType = request.PaymentType,
                Remarks = request.Remarks,
                Createat = DateTime.Now,
                TotalFee = partnerApply.OriginalPrice,
                ActuallyAmount = partnerApply.TotalAmount,
                ExtendParams = extendParams,
                IsFistOrder = true,
                OrderItems = orderItems
            };

            _orderRepository.Insert(order, false);

            using (var transaction = _orderRepository.BeginTransaction())
            {
                await _orderRepository.SaveAsync();

                transaction.Commit();
            }

            return Result.Ok(_mapper.Map<OrderCreateResponse>(order));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            var status = new OrderStatus[] { OrderStatus.Complete, OrderStatus.Canceled };
            if (!status.Contains(order.OrderStatus))
            {
                return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许删除");
            }

            await _orderRepository.RemoveAsync(order);

            return Result.Ok();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("cancel")]
        public async Task<Result> Cancel([FromBody] OrderCancelRequest request)
        {
            return await _orderService.Cancel(request.Id, request.Reason);
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("payment/{orderId}")]
        public async Task<Result> Payment(int orderId)
        {
            var order = await _orderRepository.Query().FirstOrDefaultAsync(e => e.Id == orderId);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            var status = new OrderStatus[] { OrderStatus.PendingPayment, OrderStatus.PaymentFailed };
            if (!status.Contains(order.OrderStatus))
                return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许付款");

            if (order.ActuallyAmount <= 0) return Result.Fail(ResultCodes.RequestParamError, "订单金额不能为0");

            var user = await _customerManager.GetUserAsync();
            try
            {
                var wechatResponse = await _paymentService.GeneratePaymentOrder(new GeneratePaymentOrderRequest
                {
                    OrderNo = order.OrderNo,
                    OpenId = user.OpenId,
                    TotalFee = order.ActuallyAmount,
                    Subject = "大脉商城",
                });

                var paymentTimeoutEvent = new PaymentTimeoutEvent()
                {
                    OrderId = order.Id,
                    OrderNo = order.OrderNo
                };

                var headers = new Dictionary<string, object>(){
                    {"x-delay", 10 * 60 * 1000} // 延迟10分钟
                };
                var properties = _client.CreateProperties();
                properties.Headers = headers;

                _client.PushMessage(MQConstants.Exchange, MQConstants.PaymentTimeOutRouteKey, paymentTimeoutEvent, properties);

                return Result.Ok(wechatResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Result.Fail(ResultCodes.PaymentRequestError, ex.Message);
            }
        }

        /// <summary>
        /// 账单票据
        /// </summary>
        /// <returns></returns>
        [HttpPost("billing")]
        public async Task<Result> Billing(OrderBillingRequest request)
        {
            var billing = await _orderBillingRepository.Query().FirstOrDefaultAsync(e => e.OrderId == request.OrderId);
            if (billing != null) return Result.Fail(ResultCodes.RequestParamError, "开票申请已提交，无法重复申请。");

            var anyOrder = await _orderRepository.Query().AnyAsync(e => e.Id == request.OrderId);
            if (!anyOrder) return Result.Fail(ResultCodes.IdInvalid, "订单不存在");

            billing = new OrderBilling
            {
                OrderId = request.OrderId,
                BankAccount = request.BankAccount,
                BankName = request.BankName,
                CompanyAddress = request.CompanyAddress,
                Email = request.Email,
                IsInvoiced = false,
                TaxNumber = request.TaxNumber,
                TelePhone = request.TelePhone,
                Title = request.Title,
                Createat = DateTime.Now,
            };

            await _orderBillingRepository.InsertAsync(billing);

            await _adminUserManager.AccountingNotify(new BillingNotify
            {
                Title = request.Title
            });

            return Result.Ok();
        }
    }
}