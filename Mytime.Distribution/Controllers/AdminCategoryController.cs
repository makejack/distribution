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
    /// 分类
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/category")]
    [Produces("application/json")]
    public class AdminCategoryController : ControllerBase
    {
        private readonly IRepositoryByInt<Category> _categoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="categoryRepository"></param>
        /// <param name="mapper"></param>
        public AdminCategoryController(IRepositoryByInt<Category> categoryRepository,
                                       IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminCategoryQueryRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;

            if (page <= 0)
                page = 1;

            if (limit <= 0)
            {
                limit = 10;
            }

            var queryable = _categoryRepository.Query();
            if (!string.IsNullOrEmpty(request.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(request.Name));
            }

            var totalRows = await queryable.CountAsync();
            var categorys = await queryable.Skip((page - 1) * limit).Take(limit).OrderByDescending(e => e.Id).ToListAsync();

            return Result.Ok(new PaginationResponse(page, totalRows, _mapper.Map<List<AdminCategoryResponse>>(categorys)));
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(id);
            if (category == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminCategoryResponse>(category));
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create(AdminCategoryCreateRequest request)
        {
            var category = new Category(request.Name, request.Sort);

            await _categoryRepository.InsertAsync(category);

            return Result.Ok();
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit(AdminCategoryEditRequest request)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(request.Id);
            if (category == null) return Result.Fail(ResultCodes.IdInvalid);

            category.Name = request.Name;
            category.Sort = request.Sort;

            await _categoryRepository.UpdateAsync(category);

            return Result.Ok();
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(id);
            if (category == null) return Result.Fail(ResultCodes.IdInvalid);

            await _categoryRepository.RemoveAsync(category);

            return Result.Ok();
        }
    }
}