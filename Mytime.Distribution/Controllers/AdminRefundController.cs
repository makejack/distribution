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
    [Authorize(Roles = "Admin,Accounting")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/refund")]
    public class AdminRefundController : ControllerBase
    {
        private readonly IRepositoryByInt<OrderItem> _orderItemRepository;
        private readonly IRepositoryByInt<ReturnApply> _returnApplyRepository;
        private readonly IRepositoryByInt<AdminAddress> _addressRepository;
        private readonly IRepositoryByInt<ReturnAddress> _refundAddressRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderItemRepository"></param>
        /// <param name="returnApplyRepository"></param>
        /// <param name="addressRepository"></param>
        /// <param name="refundAddressRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public AdminRefundController(IRepositoryByInt<OrderItem> orderItemRepository,
                                     IRepositoryByInt<ReturnApply> returnApplyRepository,
                                     IRepositoryByInt<AdminAddress> addressRepository,
                                     IRepositoryByInt<ReturnAddress> refundAddressRepository,
                                     ICustomerManager customerManager,
                                     IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _returnApplyRepository = returnApplyRepository;
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
            var queryable = _returnApplyRepository.Query();

            var totalRows = await queryable.CountAsync();
            var refundItems = await queryable
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .Select(e => new AdminRefundListResponse
            {
                Id = e.Id,
                AuditTime = e.AuditTime,
                Createat = e.Createat,
                Status = e.Status,
                RefundAmount = e.RefundAmount,
                CustomerId = e.CustomerId,
                CustomerName = e.Customer.NickName,
                AvatarUrl = e.Customer.AvatarUrl,
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
            var returnApply = await _returnApplyRepository.Query()
            .Include(e => e.Customer)
            .Include(e => e.OrderItem)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (returnApply == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminRefundGetResponse>(returnApply));
        }

        /// <summary>
        /// 确认申请
        /// </summary>
        /// <returns></returns>
        [HttpPost("apply")]
        public async Task<Result> ConfirmApply([FromBody] AdminRefundConfirmApplyRequest request)
        {
            var returnApply = await _returnApplyRepository.Query()
            .Include(e => e.OrderItem)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (returnApply == null) return Result.Fail(ResultCodes.IdInvalid);
            if (returnApply.Status != ReturnAuditStatus.NotAudit) return Result.Fail(ResultCodes.RequestParamError, "当前申请不允许修改状态");

            var address = await _addressRepository.Query()
            .Include(e => e.Province)
            .Include(e => e.City)
            .Include(e => e.Area)
            .FirstOrDefaultAsync(e => e.Id == request.RefundAddressId);
            if (address == null) return Result.Fail(ResultCodes.RequestParamError, "无效地址");

            var returnAddress = new ReturnAddress
            {
                ReturnApplyId = returnApply.Id,
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

            returnApply.Status = ReturnAuditStatus.Agree;
            returnApply.AuditMessage = request.Remarks;
            returnApply.AuditTime = DateTime.Now;
            returnApply.OrderItem.Status = OrderItemStatus.ConfirmApply;

            _returnApplyRepository.Update(returnApply, false);
            using (var transaction = _returnApplyRepository.BeginTransaction())
            {
                await _returnApplyRepository.SaveAsync();

                await _refundAddressRepository.InsertAsync(returnAddress);

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <returns></returns>
        [HttpPost("refuse")]
        public async Task<Result> RefuseApply([FromBody] AdminRefundRefuseApplyRequest request)
        {
            var returnApply = await _returnApplyRepository.Query()
            .Include(e => e.OrderItem)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (returnApply == null) return Result.Fail(ResultCodes.IdInvalid);
            if (returnApply.Status != ReturnAuditStatus.NotAudit) return Result.Fail(ResultCodes.RequestParamError, "当前申请不允许拒绝");

            returnApply.Status = ReturnAuditStatus.Failed;
            returnApply.AuditMessage = request.AuditMessage;
            returnApply.OrderItem.Status = OrderItemStatus.ApplyFaild;

            await _returnApplyRepository.UpdateAsync(returnApply);

            return Result.Ok();
        }

        /// <summary>
        /// 确认退款
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<Result> ConfirmRefund(int id)
        {
            var returnApply = await _returnApplyRepository.Query()
            .Include(e => e.OrderItem).ThenInclude(e => e.Order)
            .Include(e => e.OrderItem).ThenInclude(e => e.CommissionHistory)
            .Include(e => e.Shipment).ThenInclude(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (returnApply == null) return Result.Fail(ResultCodes.IdInvalid);

            var returnGoods = returnApply.OrderItem;
            var commission = returnGoods.CommissionHistory;
            if (commission != null)
            {
                commission.Status = CommissionStatus.Invalidation;
                commission.SettlementTime = DateTime.Now;

                await _customerManager.UpdateCommission(commission.CustomerId, -commission.Commission);
            }

            var order = returnGoods.Order;
            order.PaymentFee -= returnGoods.DiscountAmount;
            if (order.PaymentFee == 0)
            {
                order.OrderStatus = OrderStatus.Closed;
                order.CancelTime = DateTime.Now;
            }

            returnGoods.Status = OrderItemStatus.CompleteRefund;
            returnGoods.CompleteTime = DateTime.Now;

            var shipment = returnApply.Shipment;
            if (shipment.ShippingStatus != ShippingStatus.Complete)
            {
                var orderItems = shipment.ShipmentOrderItems.Where(e => e.OrderItemId != returnGoods.Id).Select(e => e.OrderItem);
                if (orderItems.Count() > 0)
                {
                    var count = orderItems.Count(e => e.Status == OrderItemStatus.Shipped);
                    if (count == 0)
                    {
                        shipment.ShippingStatus = ShippingStatus.Complete;
                        shipment.CompleteTime = DateTime.Now;
                    }
                }
                else
                {
                    shipment.ShippingStatus = ShippingStatus.Complete;
                    shipment.CompleteTime = DateTime.Now;
                }
            }

            returnApply.Status = ReturnAuditStatus.Completed;
            returnApply.RefundTime = DateTime.Now;

            _returnApplyRepository.Update(returnApply, false);

            using (var transaction = _returnApplyRepository.BeginTransaction())
            {
                await _returnApplyRepository.SaveAsync();

                if (returnGoods.DiscountAmount > 0)
                {
                    await _customerManager.UpdateAssets(order.CustomerId, 0, returnGoods.DiscountAmount, "退款");
                }

                transaction.Commit();
            }

            return Result.Ok();
        }
    }
}