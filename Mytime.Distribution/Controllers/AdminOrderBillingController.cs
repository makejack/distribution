using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 订单票据
    /// </summary>
    [Authorize(Roles = "Admin,Accounting")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/order/billing")]
    [Produces("application/json")]
    public class AdminOrderBillingController : ControllerBase
    {
        private readonly IRepositoryByInt<OrderBilling> _orderBillRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderBillRepository"></param>
        /// <param name="mapper"></param>
        public AdminOrderBillingController(IRepositoryByInt<OrderBilling> orderBillRepository,
                                           IMapper mapper)
        {
            _orderBillRepository = orderBillRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 票据列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] PaginationRequest request)
        {
            var queryable = _orderBillRepository.Query().Where(e => !e.IsInvoiced);

            var totalRows = await queryable.CountAsync();
            var billings = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminOrderBillingResponse>>(billings)));
        }

        /// <summary>
        /// 确认发出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<Result> ConfirmSend(int id)
        {
            var billing = await _orderBillRepository.FirstOrDefaultAsync(id);
            if (billing.IsInvoiced) return Result.Fail(ResultCodes.IdInvalid, "已开票，不得重复确认");

            billing.IsInvoiced = true;

            await _orderBillRepository.UpdateProperyAsync(billing, nameof(billing.IsInvoiced));

            return Result.Ok();
        }
    }
}