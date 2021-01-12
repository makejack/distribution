using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Events;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/customer")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IRepositoryByInt<CustomerRelation> _customerRelationRepository;
        private readonly IRepositoryByInt<Assets> _assetsRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerRepository"></param>
        /// <param name="customerRelationRepository"></param>
        /// <param name="assetsRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public CustomerController(IRepositoryByInt<Customer> customerRepository,
                                  IRepositoryByInt<CustomerRelation> customerRelationRepository,
                                  IRepositoryByInt<Assets> assetsRepository,
                                  ICustomerManager customerManager,
                                  IMapper mapper)
        {
            _customerRepository = customerRepository;
            _customerRelationRepository = customerRelationRepository;
            _assetsRepository = assetsRepository;
            _customerManager = customerManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> Get()
        {
            var user = await _customerManager.GetUserAsync();
            return Result.Ok(_mapper.Map<CustomerResponse>(user));
        }

        /// <summary>
        /// 实名
        /// </summary>
        /// <returns></returns>
        [HttpPost("realname")]
        public async Task<Result> RealName([FromBody] CustomerRealNameRequest request)
        {
            var user = await _customerManager.GetUserAsync();
            if (string.IsNullOrEmpty(user.Name)) return Result.Fail(ResultCodes.RequestParamError, "用户已实名");

            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            user.BankCode = request.BankCode;
            user.BankNo = request.BankNo;
            user.IsRealName = true;

            await _customerRepository.UpdateProperyAsync(user, nameof(user.Name), nameof(user.PhoneNumber), nameof(user.BankCode), nameof(user.BankNo), nameof(user.IsRealName));

            return Result.Ok();
        }

        /// <summary>
        /// 资产
        /// </summary>
        /// <returns></returns>
        [HttpGet("assets")]
        public async Task<Result> Assets()
        {
            var userId = HttpContext.GetUserId();
            var assets = await _assetsRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == userId);
            if (assets == null)
                assets = new Assets();

            return Result.Ok(_mapper.Map<AssetsResponse>(assets));
        }

        /// <summary>
        /// 我的团队
        /// </summary>
        /// <returns></returns>
        [HttpGet("team")]
        public async Task<Result> Team([FromQuery] PaginationRequest request)
        {
            var userId = HttpContext.GetUserId();

            var queryable = _customerRelationRepository.Query().Where(e => e.ParentId == userId);

            var totalRows = await queryable.CountAsync();
            var teams = await queryable.Include(e => e.Children).ThenInclude(e => e.Assets)
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .Select(e => e.Children)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<CustomerResponse>>(teams)));
        }
    }
}