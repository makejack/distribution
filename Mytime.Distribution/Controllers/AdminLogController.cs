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
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/log")]
    [Produces("application/json")]
    public class AdminLogController : ControllerBase
    {
        private readonly IRepositoryByInt<ErrorLog> _errorLogRepository;
        private readonly IRepositoryByInt<RequestResponseLog> _requestResponseLogRepository;
        private readonly IRepositoryByInt<SmsLog> _smsLogRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorLogRepository"></param>
        /// <param name="requestResponseLogRepository"></param>
        /// <param name="smsLogRepository"></param>
        /// <param name="mapper"></param>
        public AdminLogController(IRepositoryByInt<ErrorLog> errorLogRepository,
                                  IRepositoryByInt<RequestResponseLog> requestResponseLogRepository,
                                  IRepositoryByInt<SmsLog> smsLogRepository,
                                  IMapper mapper)
        {
            _errorLogRepository = errorLogRepository;
            _requestResponseLogRepository = requestResponseLogRepository;
            _smsLogRepository = smsLogRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 错误日志列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("error/list")]
        public async Task<Result> ErrorList([FromQuery] PaginationRequest request)
        {
            var queryable = _errorLogRepository.Query();

            var totalRows = await queryable.CountAsync();
            var errores = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminLogErrorListResponse>>(errores)));
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("error/{id}")]
        public async Task<Result> ErrorGet(int id)
        {
            var error = await _errorLogRepository.FirstOrDefaultAsync(id);
            if (error == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminLogErrorGetResponse>(error));
        }

        /// <summary>
        /// 请求响应日志列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> RequestResponselist([FromQuery] PaginationRequest request)
        {
            var queryable = _requestResponseLogRepository.Query();

            var totalRows = await queryable.CountAsync();
            var logs = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminLogListResponse>>(logs)));
        }

        /// <summary>
        /// 获取请求响应日志信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var log = await _requestResponseLogRepository.FirstOrDefaultAsync(id);
            if (log == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminLogGetResponse>(log));
        }

        /// <summary>
        /// 获取短信列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("sms/list")]
        public async Task<Result> SmsList([FromQuery] PaginationRequest request)
        {
            var queryable = _smsLogRepository.Query();

            var totalRows = await queryable.CountAsync();
            var smses = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminLogSmsListResponse>>(smses)));
        }
    }
}