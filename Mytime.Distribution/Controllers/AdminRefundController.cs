using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
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
    /// 后台退款管理
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/refund")]
    public class AdminRefundController : ControllerBase
    {
        private readonly IRepositoryByInt<OrderItem> _orderItemRepository;
        private readonly IRepositoryByInt<AdminAddress> _addressRepository;
        private readonly IRepositoryByInt<ReturnAddress> _refundAddressRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderItemRepository"></param>
        /// <param name="addressRepository"></param>
        /// <param name="refundAddressRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public AdminRefundController(IRepositoryByInt<OrderItem> orderItemRepository,
                                     IRepositoryByInt<AdminAddress> addressRepository,
                                     IRepositoryByInt<ReturnAddress> refundAddressRepository,
                                     ICustomerManager customerManager,
                                     IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository;
            _refundAddressRepository = refundAddressRepository;
            _customerManager = customerManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 退款列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] PaginationRequest request)
        {
            var queryable = _orderItemRepository.Query().Where(e => e.RefundStatus > RefundStatus.Default);

            var totalRows = await queryable.CountAsync();
            var refundItems = await queryable
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .Select(e => new AdminRefundListResponse
            {
                Id = e.Id,
                CompleteTime = e.CompleteTime,
                GoodsId = e.GoodsId,
                GoodsMediaUrl = e.GoodsMediaUrl,
                GoodsName = e.GoodsName,
                NormalizedName = e.NormalizedName,
                GoodsPrice = e.GoodsPrice,
                DiscountAmount = e.DiscountAmount,
                OrderId = e.OrderId,
                RefundAmount = e.RefundAmount,
                RefundStatus = e.RefundStatus,
                RefundApplyTime = e.RefundApplyTime,
                RefundTime = e.RefundTime,
                OrderNo = e.Order.OrderNo,
                CustomerId = e.Order.CustomerId,
                CustomerName = e.Order.Customer.NickName,
                AvatarUrl = e.Order.Customer.AvatarUrl,
            })
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, refundItems));
        }

        /// <summary>
        /// 获取退款详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var refundItem = await _orderItemRepository.Query()
            .Select(e => new AdminRefundGetResponse
            {
                Id = e.Id,
                CompleteTime = e.CompleteTime,
                GoodsId = e.GoodsId,
                GoodsMediaUrl = e.GoodsMediaUrl,
                GoodsName = e.GoodsName,
                NormalizedName = e.NormalizedName,
                GoodsPrice = e.GoodsPrice,
                DiscountAmount = e.DiscountAmount,
                OrderId = e.OrderId,
                RefundAmount = e.RefundAmount,
                RefundStatus = e.RefundStatus,
                RefundReason = e.RefundReason,
                RefundApplyTime = e.RefundApplyTime,
                RefundTime = e.RefundTime,
                OrderNo = e.Order.OrderNo,
                CustomerId = e.Order.CustomerId,
                CustomerName = e.Order.Customer.NickName,
                AvatarUrl = e.Order.Customer.AvatarUrl,
                CourierCompany = e.CourierCompany,
                CourierCompanyCode = e.CourierCompanyCode,
                TrackingNumber = e.TrackingNumber
            })
            .FirstOrDefaultAsync(e => e.Id == id);
            if (refundItem == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(refundItem);
        }

        /// <summary>
        /// 确认申请
        /// </summary>
        /// <returns></returns>
        [HttpPost("apply")]
        public async Task<Result> ConfirmApply([FromBody] AdminRefundConfirmApplyRequest request)
        {
            var refundItem = await _orderItemRepository.FirstOrDefaultAsync(request.Id);
            if (refundItem == null) return Result.Fail(ResultCodes.IdInvalid);
            if (refundItem.RefundStatus != RefundStatus.RefundApply) return Result.Fail(ResultCodes.RequestParamError, "当前申请不允许修改状态");

            var address = await _addressRepository.Query()
            .Include(e => e.Province)
            .Include(e => e.City)
            .Include(e => e.Area)
            .FirstOrDefaultAsync(e => e.Id == request.RefundAddressId);
            if (address == null) return Result.Fail(ResultCodes.RequestParamError, "无效地址");

            var returnAddress = new ReturnAddress
            {
                OrderItemId = refundItem.Id,
                AreaName = address.Area.Name,
                CityName = address.City.Name,
                Createat = DateTime.Now,
                DetailInfo = address.DetailInfo,
                PostalCode = address.PostalCode.ToString(),
                ProvinceName = address.Province.Name,
                Remarks = request.Remarks,
                TelNumber = address.TelNumber,
                UserName = address.UserName,
            };

            refundItem.RefundStatus = RefundStatus.ConfirmApply;

            _orderItemRepository.Update(refundItem, false);
            using (var transaction = _orderItemRepository.BeginTransaction())
            {
                await _orderItemRepository.SaveAsync();

                await _refundAddressRepository.InsertAsync(returnAddress);

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 确认退款
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<Result> ConfirmRefund(int id)
        {
            var refundItem = await _orderItemRepository.Query()
            // .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.Shipment)
            .Include(e => e.Order)
            .Include(e => e.CommissionHistory)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (refundItem == null) return Result.Fail(ResultCodes.IdInvalid);

            var commission = refundItem.CommissionHistory;
            if (commission != null)
            {
                commission.Status = CommissionStatus.Invalidation;
                commission.SettlementTime = DateTime.Now;

                await _customerManager.UpdateAssets(commission.CustomerId, -commission.Commission, 0);
            }

            var order = refundItem.Order;
            order.PaymentFee -= refundItem.DiscountAmount;
            if (order.PaymentFee == 0)
            {
                order.OrderStatus = OrderStatus.Closed;
                order.CancelTime = DateTime.Now;
            }

            // var shipments = refundItem.ShipmentOrderItems.Select(e => e.Shipment);
            // foreach (var item in shipments)
            // {
            //     if (item.IsValid) continue;
            //     item.ShippingStatus = ShippingStatus.Complete;
            //     item.CompleteTime = DateTime.Now;
            // }

            refundItem.RefundStatus = RefundStatus.CompleteRefund;
            refundItem.RefundTime = DateTime.Now;
            refundItem.CompleteTime = DateTime.Now;

            _orderItemRepository.Update(refundItem, false);

            using (var transaction = _orderItemRepository.BeginTransaction())
            {
                await _orderItemRepository.SaveAsync();

                transaction.Commit();
            }

            return Result.Ok();
        }
    }
}