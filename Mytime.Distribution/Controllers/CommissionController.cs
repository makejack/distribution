using System.Collections.Generic;
using System.Net.Cache;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 佣金
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/commission")]
    [Produces("application/json")]
    public class CommissionController : ControllerBase
    {
        private readonly IRepositoryByInt<CommissionHistory> _commissionHistoryRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commissionHistoryRepository"></param>
        /// <param name="mapper"></param>
        public CommissionController(IRepositoryByInt<CommissionHistory> commissionHistoryRepository,
                                    IMapper mapper)
        {
            _commissionHistoryRepository = commissionHistoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("history")]
        public async Task<Result> History([FromQuery] PaginationRequest request)
        {
            var userId = HttpContext.GetUserId();

            var queryable = _commissionHistoryRepository.Query().Where(e => e.CustomerId == userId);
            var totalRows = await queryable.CountAsync();
            var historys = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit).ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<CommissionHistoryResponse>>(historys)));
        }
    }
}