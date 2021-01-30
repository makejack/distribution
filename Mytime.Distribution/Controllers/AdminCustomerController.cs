using System.Net;
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
    /// 后台顾客
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/customer")]
    [Produces("application/json")]
    public class AdminCustomerController : ControllerBase
    {
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerRepository"></param>
        /// <param name="mapper"></param>
        public AdminCustomerController(IRepositoryByInt<Customer> customerRepository,
                                       IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<Result> GetAll([FromQuery] string nickName)
        {
            var customers = new List<Customer>();
            if (!string.IsNullOrEmpty(nickName))
            {
                customers = await _customerRepository.Query().Where(e => e.NickName.Contains(nickName)).ToListAsync();
            }
            return Result.Ok(_mapper.Map<List<CustomerResponse>>(customers));
        }

        /// <summary>
        /// 顾客列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminCustomerListRequest request)
        {
            var queryable = _customerRepository.Query();

            if (request.Role.HasValue)
            {
                queryable = queryable.Where(e => e.Role == request.Role.Value);
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(request.Name));
            }
            if (!string.IsNullOrEmpty(request.Tel))
            {
                queryable = queryable.Where(e => e.PhoneNumber.Contains(request.Tel));
            }

            var totalRows = await queryable.CountAsync();
            var customers = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<CustomerResponse>>(customers)));
        }

        /// <summary>
        /// 顾客信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var customer = await _customerRepository.Query()
            .Include(e => e.Assets)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (customer == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminCustomerGetResponse>(customer));
        }
    }
}