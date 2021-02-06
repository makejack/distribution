using System.Runtime.InteropServices;
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
    /// 商品选项
    /// </summary>
    [Authorize(Roles = "Admin,Goods")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/goods/option")]
    [Produces("application/json")]
    public class AdminGoodsOptionController : ControllerBase
    {
        private readonly IRepositoryByInt<GoodsOption> _goodsOptionRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goodsOptionRepository"></param>
        /// <param name="mapper"></param>
        public AdminGoodsOptionController(IRepositoryByInt<GoodsOption> goodsOptionRepository,
                                          IMapper mapper)
        {
            _goodsOptionRepository = goodsOptionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有商品选项
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<Result> All()
        {
            var options = await _goodsOptionRepository.Query().ToListAsync();

            return Result.Ok(_mapper.Map<List<AdminGoodsOptionResponse>>(options));
        }

        /// <summary>
        /// 商品选项列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminGoodsOptionQueryRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            var queryable = _goodsOptionRepository.Query();
            if (!string.IsNullOrEmpty(request.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(request.Name));
            }

            var totalRows = await queryable.CountAsync();
            var goodsOptions = await queryable.OrderByDescending(e => e.Id).Skip((page - 1) * limit).Take(limit).ToListAsync();
            var pagination = new PaginationResponse(page, totalRows, _mapper.Map<List<AdminGoodsOptionResponse>>(goodsOptions));
            return Result.Ok(pagination);
        }

        /// <summary>
        /// 获取商品选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var goodsOption = await _goodsOptionRepository.FirstOrDefaultAsync(id);
            if (goodsOption == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminGoodsOptionResponse>(goodsOption));
        }

        /// <summary>
        /// 创建商品选项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] AdminGoodsOptionCreateRequest request)
        {
            var goodsOption = new GoodsOption(request.Name);

            await _goodsOptionRepository.InsertAsync(goodsOption);

            return Result.Ok();
        }

        /// <summary>
        /// 编辑商品选项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit([FromBody] AdminGoodsOptionEditRequest request)
        {
            var goodsOption = await _goodsOptionRepository.FirstOrDefaultAsync(request.Id);
            if (goodsOption == null) return Result.Fail(ResultCodes.IdInvalid);

            goodsOption.Name = request.Name;

            await _goodsOptionRepository.UpdateAsync(goodsOption);

            return Result.Ok();
        }

        /// <summary>
        /// 删除商品选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var goodsOption = await _goodsOptionRepository.Query()
            .Include(e => e.GoodsOptionCombinations)
            .Include(e => e.GoodsOptionValues)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (goodsOption == null) return Result.Fail(ResultCodes.IdInvalid);

            if (goodsOption.GoodsOptionCombinations.Count > 0)
            {
                return Result.Fail(ResultCodes.RequestParamError, "当前商品存在商品组合，不允许删除");
            }

            if (goodsOption.GoodsOptionValues.Count > 0)
            {
                return Result.Fail(ResultCodes.RequestParamError, "当前商品存在商品组合，不允许删除");
            }

            await _goodsOptionRepository.RemoveAsync(goodsOption);

            return Result.Ok();
        }
    }
}