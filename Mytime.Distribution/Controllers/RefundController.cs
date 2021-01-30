using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Utils.Helpers;
using Mytime.Distribution.Services;
using Mytime.Distribution.Services.SmsContent;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 退款
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/return")]
    [Produces("application/json")]
    public class RefundController : ControllerBase
    {
        private readonly IRepositoryByInt<OrderItem> _orderItemRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IAdminUserManager _adminUserManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderItemRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="adminUserManager"></param>
        /// <param name="mapper"></param>
        public RefundController(IRepositoryByInt<OrderItem> orderItemRepository,
                                ICustomerManager customerManager,
                                IAdminUserManager adminUserManager,
                                IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _customerManager = customerManager;
            _adminUserManager = adminUserManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 退货详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RefundGetResponse), 200)]
        public async Task<Result> Get(int id)
        {
            var userId = HttpContext.GetUserId();

            var item = await _orderItemRepository.Query()
            .Include(e => e.ReturnApply).ThenInclude(e => e.ReturnAddress)
            // .Where(e => e.Order.CustomerId == userId && e.RefundStatus != RefundStatus.Default)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<RefundGetResponse>(item));
        }

        /// <summary>
        /// 退货申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Apply(ReturnApplyRequest request)
        {
            var item = await _orderItemRepository.Query()
            .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.Shipment)
            .Include(e => e.ReturnApply)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);
            if (item.IsFirstBatchGoods) return Result.Fail(ResultCodes.RequestParamError, "首批商品不允许退货");
            if (item.Status != OrderItemStatus.Shipped) return Result.Fail(ResultCodes.RequestParamError, "当前商品状态不允许退货");
            if (item.Status == OrderItemStatus.RefundApply) return Result.Fail(ResultCodes.RequestParamError, "当前商品已申请退货");

            var user = await _customerManager.GetUserAsync();
            if (item.ReturnApply == null)
            {
                var shipment = item.ShipmentOrderItems.Where(e => e.Shipment.IsValid).Select(e => e.Shipment).FirstOrDefault();
                item.ReturnApply = new ReturnApply
                {
                    CustomerId = user.Id,
                    Createat = DateTime.Now,
                    Reason = request.Reason,
                    Description = string.Empty,
                    LogisticsStatus = request.LogisticsStatus,
                    ReturnType = request.ReturnType,
                    PaymentAmount = item.DiscountAmount,
                    RefundAmount = item.DiscountAmount,
                    Status = ReturnAuditStatus.NotAudit,
                    ShipmentId = shipment.Id
                };
            }
            else
            {
                var returnApply = item.ReturnApply;
                returnApply.Createat = DateTime.Now;
                returnApply.Reason = request.Reason;
                returnApply.LogisticsStatus = request.LogisticsStatus;
                returnApply.ReturnType = request.ReturnType;
                returnApply.Status = ReturnAuditStatus.NotAudit;
            }

            item.Status = OrderItemStatus.RefundApply;

            _orderItemRepository.Update(item, false);

            using (var transaction = _orderItemRepository.BeginTransaction())
            {
                await _orderItemRepository.SaveAsync();

                transaction.Commit();
            }

            await _adminUserManager.AccountingNotify(new ReturnNotify
            {
                UserName = user.NickName
            });

            return Result.Ok();
        }

        /// <summary>
        /// 取消退货申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Cancel(int id)
        {
            var item = await _orderItemRepository.Query()
            .Include(e => e.ReturnApply)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);
            var status = new[] { OrderItemStatus.RefundApply, OrderItemStatus.ConfirmApply };
            if (!status.Contains(item.Status)) return Result.Fail(ResultCodes.RequestParamError, "当前商品状态不允许取消申请");

            item.Status = OrderItemStatus.Shipped;
            item.ReturnApply.Status = ReturnAuditStatus.Cancel;

            await _orderItemRepository.UpdateAsync(item);

            return Result.Ok();
        }

        /// <summary>
        /// 设置快递单号
        /// </summary>
        /// <returns></returns>
        [HttpPost("settrackingnumber")]
        public async Task<Result> SetTrackingNumber([FromBody] RefundSetTrackingNumberRequest request)
        {
            var item = await _orderItemRepository.Query()
            .Include(e => e.ReturnApply)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);
            if (item.Status != OrderItemStatus.ConfirmApply) return Result.Fail(ResultCodes.RequestParamError, "当前状态不允许设置快递单号");

            var returnApply = item.ReturnApply;
            if (string.IsNullOrEmpty(request.CourierCompany))
            {
                returnApply.CourierCompany = CourierCompanyHelper.GetCompanyName(request.CourierCompanyCode);
            }
            else
            {
                returnApply.CourierCompany = request.CourierCompany;
            }

            returnApply.Status = ReturnAuditStatus.ReturnGoods;
            returnApply.CourierCompanyCode = request.CourierCompanyCode;
            returnApply.TrackingNumber = request.TrackingNumber;

            item.Status = OrderItemStatus.ReturnGoods;

            await _orderItemRepository.UpdateAsync(item);

            return Result.Ok();
        }
    }
}