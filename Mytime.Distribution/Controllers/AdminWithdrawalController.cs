using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
    /// 后台提现
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/withdrawal")]
    [Produces("application/json")]
    public class AdminWithdrawalController : ControllerBase
    {
        private readonly IRepositoryByInt<WithdrawalHistory> _withdrawalHistoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="withdrawalHistoryRepository"></param>
        /// <param name="mapper"></param>
        public AdminWithdrawalController(IRepositoryByInt<WithdrawalHistory> withdrawalHistoryRepository,
                                         IMapper mapper)
        {
            _withdrawalHistoryRepository = withdrawalHistoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        public async Task<Result> History([FromQuery] AdminWithdrawalHistoryRequest request)
        {
            var queryable = _withdrawalHistoryRepository.Query();

            if (request.CustomerId.HasValue)
            {
                queryable = queryable.Where(e => e.CustomerId == request.CustomerId.Value);
            }

            var totalRows = await queryable.CountAsync();
            var histores = await queryable
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<WithdrawalHistoryResponse>>(histores)));
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] PaginationRequest request)
        {
            var queryable = _withdrawalHistoryRepository.Query();

            var totalRows = await queryable.CountAsync();
            var histores = await queryable.Include(e => e.Customer)
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminWithdrawalHistoryResponse>>(histores)));
        }
    }
}