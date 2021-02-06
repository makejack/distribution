using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台佣金
    /// </summary>
    [Authorize(Roles = "Admin,Accounting")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/commission")]
    [Produces("application/json")]
    public class AdminCommissionController : ControllerBase
    {
        private readonly IRepositoryByInt<CommissionHistory> _commissionHistoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commissionHistoryRepository"></param>
        /// <param name="mapper"></param>
        public AdminCommissionController(IRepositoryByInt<CommissionHistory> commissionHistoryRepository,
                                         IMapper mapper)
        {
            _commissionHistoryRepository = commissionHistoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 佣金列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        public async Task<Result> History([FromQuery] AdminCommissionHistoryRequest request)
        {
            var queryable = _commissionHistoryRepository.Query();

            if (request.CustomerId.HasValue)
            {
                queryable = queryable.Where(e => e.CustomerId == request.CustomerId.Value);
            }
            if (request.Status.HasValue)
            {
                queryable = queryable.Where(e => e.Status == request.Status.Value);
            }

            var totalRows = await queryable.CountAsync();
            var histores = await queryable
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<CommissionHistoryResponse>>(histores)));
        }

        /// <summary>
        /// 佣金列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminCommissionHistoryRequest request)
        {
            var queryable = _commissionHistoryRepository.Query();

            if (request.Status.HasValue)
            {
                queryable = queryable.Where(e => e.Status == request.Status);
            }

            var totalRows = await queryable.CountAsync();
            var histores = await queryable
            .Include(e => e.Customer)
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminCommissionHistoryResponse>>(histores)));
        }
    }
}