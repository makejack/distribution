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
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderItemRepository"></param>
        /// <param name="mapper"></param>
        public RefundController(IRepositoryByInt<OrderItem> orderItemRepository,
                                IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
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
            .Include(e => e.ReturnAddress)
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
            var item = await _orderItemRepository.FirstOrDefaultAsync(request.Id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);
            if (item.IsFirstBatchGoods) return Result.Fail(ResultCodes.RequestParamError, "首批商品不允许退货");
            if (item.ShippingStatus != ShippingStatus.Shipped) return Result.Fail(ResultCodes.RequestParamError, "当前商品状态不允许退货");
            if (item.RefundStatus == RefundStatus.RefundApply) return Result.Fail(ResultCodes.RequestParamError, "当前商品已申请退货");
            var status = new[] { RefundStatus.ApplyFaild };
            if (item.RefundStatus != RefundStatus.Default) return Result.Fail(ResultCodes.RequestParamError, "当前商品不允许申请退货");

            item.RefundStatus = RefundStatus.RefundApply;
            item.RefundReason = request.Reason;
            item.RefundAmount = item.DiscountAmount;
            item.RefundApplyTime = DateTime.Now;

            await _orderItemRepository.UpdateAsync(item);

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
            var item = await _orderItemRepository.FirstOrDefaultAsync(id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);
            var status = new[] { RefundStatus.RefundApply, RefundStatus.ConfirmApply };
            if (!status.Contains(item.RefundStatus)) return Result.Fail(ResultCodes.RequestParamError, "当前商品状态不允许取消申请");

            item.RefundStatus = RefundStatus.Default;

            await _orderItemRepository.UpdateProperyAsync(item, nameof(item.RefundStatus));

            return Result.Ok();
        }

        /// <summary>
        /// 设置快递单号
        /// </summary>
        /// <returns></returns>
        [HttpPost("settrackingnumber")]
        public async Task<Result> SetTrackingNumber([FromBody] RefundSetTrackingNumberRequest request)
        {
            var item = await _orderItemRepository.FirstOrDefaultAsync(request.Id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);
            if (item.RefundStatus != RefundStatus.ConfirmApply) return Result.Fail(ResultCodes.RequestParamError, "当前状态不允许设置快递单号");

            if (string.IsNullOrEmpty(request.CourierCompany))
            {
                item.CourierCompany = CourierCompanyHelper.GetCompanyName(request.CourierCompanyCode);
            }
            else
            {
                item.CourierCompany = request.CourierCompany;
            }

            item.ShippingStatus = ShippingStatus.ReturnGoods;
            item.CourierCompanyCode = request.CourierCompanyCode;
            item.TrackingNumber = request.TrackingNumber;
            item.RefundStatus = RefundStatus.ReturnGoods;

            await _orderItemRepository.UpdateAsync(item);

            return Result.Ok();
        }
    }
}