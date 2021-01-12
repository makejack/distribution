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

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 退货
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/return")]
    [Produces("application/json")]
    public class ReturnController : ControllerBase
    {
        private readonly IRepositoryByInt<OrderItem> _orderItemRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderItemRepository"></param>
        /// <param name="mapper"></param>
        public ReturnController(IRepositoryByInt<OrderItem> orderItemRepository,
                                IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        /*
        /// <summary>
        /// 退货申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Apply(ReturnApplyRequest request)
        {
            var item = await _orderItemRepository.FirstOrDefaultAsync(request.Id);
            if (item == null) return Result.Fail(ResultCodes.IdInvalid);

            if (item.ShippingStatus == ShippingStatus.ReturnApply) return Result.Fail(ResultCodes.RequestParamError, "当前商品已申请退货");
            var status = new[] { ShippingStatus.ApplyFaild, ShippingStatus.Shipped };
            if (!status.Contains(item.ShippingStatus)) return Result.Fail(ResultCodes.RequestParamError, "当前商品不允许申请退货");

            item.ShippingStatus = ShippingStatus.ReturnApply;
            item.ReturnReason = request.Reason;
            item.ReturnTime = DateTime.Now;

            await _orderItemRepository.UpdateAsync(item);

            return Result.Ok();
        }
        */
    }
}