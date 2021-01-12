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
    /// 后台订单管理
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/order")]
    [Produces("application/json")]
    public class AdminOrderController : ControllerBase
    {
        private readonly IRepositoryByInt<Orders> _orderRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="mapper"></param>
        public AdminOrderController(IRepositoryByInt<Orders> orderRepository,
                                    IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminOrderListRequest request)
        {
            var queryable = _orderRepository.Query();

            if (request.CustomerId.HasValue)
            {
                queryable = queryable.Where(e => e.CustomerId == request.CustomerId.Value);
            }
            if (request.Status.HasValue)
            {
                queryable = queryable.Where(e => e.OrderStatus == request.Status.Value);
            }

            var totalRows = await queryable.CountAsync();
            var orders = await queryable.OrderByDescending(e => e.Id).Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminOrderListResponse>>(orders)));
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var order = await _orderRepository.Query().Include(e => e.OrderItems).FirstOrDefaultAsync(e => e.Id == id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminOrderGetResponse>(order));
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(id);
            if (order == null) return Result.Fail(ResultCodes.IdInvalid);

            await _orderRepository.RemoveAsync(order);

            return Result.Ok();
        }
    }
}