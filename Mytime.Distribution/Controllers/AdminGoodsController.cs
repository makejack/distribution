using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Linq;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
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
    /// 后台商品
    /// </summary>
    [Authorize(Roles = "Admin,Goods")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/goods")]
    [Produces("application/json")]
    public class AdminGoodsController : ControllerBase
    {
        private readonly IRepositoryByInt<Goods> _goodsRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goodsRepository"></param>
        /// <param name="mapper"></param>
        public AdminGoodsController(IRepositoryByInt<Goods> goodsRepository,
                                    IMapper mapper)
        {
            _goodsRepository = goodsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminGoodsListQueryRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            var queryable = _goodsRepository.Query()
            .Include(e => e.ThumbnailImage)
            .Where(e => e.IsVisible == false);
            if (!string.IsNullOrEmpty(request.Name))
            {
                queryable = queryable.Where(e => e.Name.Contains(request.Name));
            }
            if (request.IsPublished.HasValue)
            {
                queryable = queryable.Where(e => e.IsPublished == request.IsPublished.Value);
            }

            var totalRows = await queryable.CountAsync();
            var goodses = await queryable.OrderByDescending(e => e.Id).Skip((page - 1) * limit).Take(limit).ToListAsync();
            var pagination = new PaginationResponse(page, totalRows, _mapper.Map<List<AdminGoodsListResponse>>(goodses));

            return Result.Ok(pagination);
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<Result> Get(int id)
        {
            var goods = await _goodsRepository.Query()
            .Include(e => e.ThumbnailImage)
            .Include(e => e.GoodsMedias).ThenInclude(e => e.Media)
            .Include(e => e.Childrens).ThenInclude(e => e.OptionCombinations).ThenInclude(e => e.Option)
            .Include(e => e.Childrens)
            .Include(e => e.OptionValues).ThenInclude(e => e.Option)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (goods == null) return Result.Fail(ResultCodes.IdInvalid);

            var goodsRes = _mapper.Map<AdminGoodsGetResponse>(goods);

            var optionIds = goods.OptionValues.OrderBy(e => e.DisplayOrder).ThenBy(x => x.Option.Name).GroupBy(c => c.OptionId).Select(s => s.Key).OrderBy(c => c);
            var options = new List<AdminGoodsGetOptionResponse>();
            foreach (var optionId in optionIds)
            {
                var first = goods.OptionValues.First(c => c.OptionId == optionId);
                var result = new AdminGoodsGetOptionResponse
                {
                    Id = first.OptionId,
                    Name = first.Option.Name,
                    Createat = first.Createat,
                    Values = goods.OptionValues.Where(e => e.OptionId == optionId).Select(s => new AdminGoodsGetOptionValueResponse
                    {
                        Id = s.Id,
                        DisplayOrder = s.DisplayOrder,
                        Value = s.Value,
                        Createat = s.Createat
                    }).OrderBy(c => c.DisplayOrder).ToList()
                };
                options.Add(result);
            }
            goodsRes.Options = options;

            goodsRes.Variations = goods.Childrens.Select(s => new AdminGoodsGetVariationResponse
            {
                Id = s.Id,
                NormalizedName = s.NormalizedName,
                StockQuantity = s.StockQuantity,
                Price = s.Price,
                OptionCombinations = s.OptionCombinations.Select(e => new AdminGoodsGetOptionCombinationResponse
                {
                    OptionId = e.OptionId,
                    OptionName = e.Option?.Name,
                    Value = e.Value,
                    DisplayOrder = e.DisplayOrder
                }).OrderBy(e => e.DisplayOrder).ToList()
            }).ToList();

            return Result.Ok(goodsRes);
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] AdminGoodsCreateRequest request)
        {
            var goodsMedias = new List<GoodsMedia>();
            if (request.GoodsMediaIds != null && request.GoodsMediaIds.Count > 0)
            {
                foreach (var mediaId in request.GoodsMediaIds)
                {
                    goodsMedias.Add(new GoodsMedia(mediaId));
                }
            }
            var goods = new Goods
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ThumbnailImageId = request.ThumbnailImageId,
                IsPublished = request.IsPublished,
                HasOptions = true,
                IsVisible = false,
                BranchDiscount = request.BranchDiscount,
                CityDiscount = request.CityDiscount,
                DisplayOrder = request.DisplayOrder,
                GoodsMedias = goodsMedias,
                Createat = DateTime.Now
            };
            if (request.IsPublished)
            {
                goods.PublishedOn = DateTime.Now;
            }

            List<GoodsOptionValue> goodsOptionValues = new List<GoodsOptionValue>();
            foreach (var option in request.Options.Distinct())
            {
                foreach (var item in option.Values.Distinct())
                {
                    goodsOptionValues.Add(new GoodsOptionValue(option.Id, item.Value, item.DisplayOrder));
                }
            }
            goods.OptionValues = goodsOptionValues;

            List<Goods> childrens = new List<Goods>();
            var variations = request.Variations.Distinct();
            foreach (var variation in variations)
            {
                var optionCombinations = new List<GoodsOptionCombination>();
                var coms = variation.OptionCombinations.Distinct();
                foreach (var combination in coms)
                {
                    if (!goods.OptionValues.Any(c => c.OptionId == combination.OptionId))
                    {
                        return Result.Fail(ResultCodes.RequestParamError, "商品组合中的选项不存在");
                    }
                    if (!goods.OptionValues.Any(c => c.Value == combination.Value))
                    {
                        return Result.Fail(ResultCodes.RequestParamError, "商品组合中的选项值不存在");
                    }
                    if (optionCombinations.Any(c => c.OptionId == combination.OptionId && c.Value == combination.Value))
                        continue;

                    optionCombinations.Add(new GoodsOptionCombination(combination.OptionId, combination.DisplayOrder, combination.Value));
                }
                var child = new Goods
                {
                    Name = goods.Name + variation.NormalizedName,
                    NormalizedName = variation.NormalizedName,
                    Price = variation.Price,
                    HasOptions = false,
                    IsVisible = true,
                    StockQuantity = variation.StockQuantity,
                    Createat = DateTime.Now,
                    OptionCombinations = optionCombinations
                };
                if (request.IsPublished)
                {
                    child.IsPublished = true;
                    child.PublishedOn = DateTime.Now;
                }
                childrens.Add(child);
            }
            goods.Childrens = childrens;

            _goodsRepository.Insert(goods, false);

            using (var transaction = _goodsRepository.BeginTransaction())
            {
                await _goodsRepository.SaveAsync();

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<Result> Edit(int id, [FromBody] AdminGoodsCreateRequest request)
        {
            var goods = await _goodsRepository.Query()
            .Include(e => e.GoodsMedias)
            .Include(e => e.Childrens).ThenInclude(e => e.OptionCombinations)
            .Include(e => e.Childrens)
            .Include(e => e.OptionValues).ThenInclude(e => e.Option)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (goods == null) return Result.Fail(ResultCodes.IdInvalid);

            var goodsMedias = new List<GoodsMedia>();
            if (request.GoodsMediaIds != null && request.GoodsMediaIds.Count > 0)
            {
                foreach (var mediaId in request.GoodsMediaIds)
                {
                    goodsMedias.Add(new GoodsMedia(mediaId));
                }
            }

            goods.Name = request.Name;
            goods.Description = request.Description;
            goods.Price = request.Price;
            goods.ThumbnailImageId = request.ThumbnailImageId;
            goods.DisplayOrder = request.DisplayOrder;
            goods.CityDiscount = request.CityDiscount;
            goods.BranchDiscount = request.BranchDiscount;
            goods.GoodsMedias = goodsMedias;

            if (request.IsPublished)
            {
                if (!goods.IsPublished)
                {
                    goods.IsPublished = request.IsPublished;
                    goods.PublishedOn = DateTime.Now;
                }
            }
            else
            {
                goods.PublishedOn = null;
                goods.IsPublished = false;
            }

            var optionValues = new List<GoodsOptionValue>();
            var options = request.Options.Distinct();
            foreach (var option in options)
            {
                foreach (var item in option.Values.Distinct())
                {
                    var ov = goods.OptionValues.FirstOrDefault(c => c.OptionId == option.Id && c.Value == item.Value);
                    if (ov == null)
                    {
                        optionValues.Add(new GoodsOptionValue(option.Id, item.Value, item.DisplayOrder));
                    }
                    else
                    {
                        ov.DisplayOrder = item.DisplayOrder;
                        optionValues.Add(ov);
                    }
                }
            }
            goods.OptionValues = optionValues;

            var variations = request.Variations.Distinct();
            var childrens = new List<Goods>();
            foreach (var variation in variations)
            {
                Goods child = null;
                if (variation.Id > 0)
                {
                    child = goods.Childrens.FirstOrDefault(e => e.Id == variation.Id);
                }

                if (child == null)
                {
                    var optionCombinations = new List<GoodsOptionCombination>();
                    var coms = variation.OptionCombinations.Distinct();
                    foreach (var combination in coms)
                    {
                        if (!goods.OptionValues.Any(c => c.OptionId == combination.OptionId))
                        {
                            return Result.Fail(ResultCodes.RequestParamError, "商品组合中的选项不存在");
                        }
                        if (!goods.OptionValues.Any(c => c.Value == combination.Value))
                        {
                            return Result.Fail(ResultCodes.RequestParamError, "商品组合中的选项值不存在");
                        }
                        if (optionCombinations.Any(c => c.OptionId == combination.OptionId && c.Value == combination.Value))
                            continue;

                        optionCombinations.Add(new GoodsOptionCombination
                        {
                            OptionId = combination.OptionId,
                            Value = combination.Value,
                            DisplayOrder = combination.DisplayOrder,
                            Createat = DateTime.Now
                        });
                    }

                    child = new Goods
                    {
                        Name = goods.Name + variation.NormalizedName,
                        NormalizedName = variation.NormalizedName,
                        Price = variation.Price,
                        HasOptions = false,
                        IsVisible = true,
                        StockQuantity = variation.StockQuantity,
                        Createat = DateTime.Now,
                        OptionCombinations = optionCombinations
                    };
                }
                else
                {
                    child.Name = request.Name + variation.NormalizedName;
                    child.NormalizedName = variation.NormalizedName;
                    child.Price = variation.Price;
                    child.StockQuantity = variation.StockQuantity;
                }
                childrens.Add(child);
            }
            var deleteChildrens = goods.Childrens.Where(x => variations.All(c => c.Id != x.Id));
            foreach (var deleteChild in deleteChildrens)
            {
                _goodsRepository.Remove(deleteChild, false);
            }

            goods.Childrens = childrens;

            if (!goods.IsPublished)
            {
                foreach (var item in goods.Childrens.Where(e => e.IsPublished))
                {
                    item.IsPublished = false;
                    item.PublishedOn = null;
                }
            }

            _goodsRepository.Update(goods, false);

            using (var transaction = _goodsRepository.BeginTransaction())
            {
                await _goodsRepository.SaveAsync();

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var goods = await _goodsRepository.FirstOrDefaultAsync(id);
            if (goods == null) return Result.Fail(ResultCodes.IdInvalid);

            var anyChild = await _goodsRepository.Query().AnyAsync(c => c.ParentId == id);
            if (anyChild)
            {
                return Result.Fail(ResultCodes.RequestParamError, "当前商品存在商品组合，不允许删除");
            }

            await _goodsRepository.RemoveAsync(goods);

            return Result.Ok();
        }
    }
}