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
    /// 商品选项数据
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/goods/optiondata")]
    [Produces("application/json")]
    public class AdminGoodsOptionDataController : ControllerBase
    {
        private readonly IRepositoryByInt<GoodsOptionData> _goodsOptionDataRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goodsOptionDataRepository"></param>
        /// <param name="mapper"></param>
        public AdminGoodsOptionDataController(IRepositoryByInt<GoodsOptionData> goodsOptionDataRepository,
                                              IMapper mapper)
        {
            _goodsOptionDataRepository = goodsOptionDataRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有商品选项值
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<Result> All()
        {
            var datas = await _goodsOptionDataRepository.Query().ToListAsync();
            return Result.Ok(_mapper.Map<List<AdminGoodsOptionDataResponse>>(datas));
        }

        /// <summary>
        /// 商品选项数据列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminGoodsOptionDataQueryRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;
            if (page <= 0)
            {
                page = 1;
            }
            if (limit <= 0)
            {
                limit = 10;
            }

            var queryable = _goodsOptionDataRepository.Query();
            if (request.OptionId > 0)
            {
                queryable = queryable.Where(e => e.OptionId == request.OptionId);
            }
            if (!string.IsNullOrEmpty(request.Value))
            {
                queryable = queryable.Where(e => e.Value.Contains(request.Value));
            }

            var totalRows = await queryable.CountAsync();
            var goodsOptionDatas = await queryable.OrderByDescending(e => e.Id).Skip((page - 1) * limit).Take(limit).ToListAsync();
            var pagination = new PaginationResponse(page, totalRows, _mapper.Map<List<AdminGoodsOptionDataResponse>>(goodsOptionDatas));

            return Result.Ok(pagination);
        }

        /// <summary>
        /// 获取商品选项数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var goodsOptionData = await _goodsOptionDataRepository.FirstOrDefaultAsync(id);
            if (goodsOptionData == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminGoodsOptionDataResponse>(goodsOptionData));
        }

        /// <summary>
        /// 创建商品选项数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] AdminGoodsOptionDataCreateRequest request)
        {
            var goodsOptionData = new GoodsOptionData(request.OptionId, request.Value, request.Description);

            await _goodsOptionDataRepository.InsertAsync(goodsOptionData);

            return Result.Ok();
        }

        /// <summary>
        /// 编辑商品选项数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit([FromBody] AdminGoodsOptionDataEditRequest request)
        {
            var goodsOptionData = await _goodsOptionDataRepository.FirstOrDefaultAsync(request.Id);
            if (goodsOptionData == null) return Result.Fail(ResultCodes.IdInvalid);

            goodsOptionData.Value = request.Value;
            goodsOptionData.Description = request.Description;

            await _goodsOptionDataRepository.UpdateAsync(goodsOptionData);

            return Result.Ok();
        }

        /// <summary>
        /// 删除商品选项数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var goodsOptionData = await _goodsOptionDataRepository.FirstOrDefaultAsync(id);
            if (goodsOptionData == null) return Result.Fail(ResultCodes.IdInvalid);

            await _goodsOptionDataRepository.RemoveAsync(goodsOptionData);

            return Result.Ok();
        }
    }
}