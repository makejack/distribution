using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 产品
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/goods")]
    [Produces("application/json")]
    public class GoodsController : ControllerBase
    {
        private readonly IRepositoryByInt<Goods> _goodsRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goodsRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public GoodsController(IRepositoryByInt<Goods> goodsRepository,
                               ICustomerManager customerManager,
                               IMapper mapper)
        {
            _goodsRepository = goodsRepository;
            _customerManager = customerManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Authorize]
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] PaginationRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;

            var queryable = _goodsRepository.Query().Where(e => e.IsPublished && !e.IsVisible);

            var totalRows = await queryable.CountAsync();
            var goodsList = await queryable
            .Include(e => e.ThumbnailImage)
            .Include(e => e.GoodsMedias).ThenInclude(e => e.Media)
            .OrderBy(e => e.DisplayOrder)
            .Skip((page - 1) * limit).Take(limit)
            .ToListAsync();

            var goodsRes = _mapper.Map<List<GoodsListResponse>>(goodsList);

            var discountRate = 100f;
            Customer user = null;
            var authenticationScheme = JwtBearerDefaults.AuthenticationScheme;
            var auth = HttpContext.AuthenticateAsync(authenticationScheme);
            if (auth.IsCompletedSuccessfully)
            {
                user = await _customerManager.GetUserAsync();
            }
            // if (user.Role != PartnerRole.Default)
            // {
            //     var partnerConfig = _customerManager.GetUserPartnerConfig(user.Role);
            //     discountRate = partnerConfig.DiscountRate / 100f;
            // }

            for (int i = 0; i < goodsList.Count; i++)
            {
                var goods = goodsList[i];
                if (user != null)
                {
                    if (user.Role == PartnerRole.CityPartner && goods.CityDiscount > 0)
                        discountRate = goods.CityDiscount;
                    else if (user.Role == PartnerRole.BranchPartner && goods.BranchDiscount > 0)
                        discountRate = goods.BranchDiscount;
                }
                goodsRes[i].DiscountPrice = (int)(goods.Price * (discountRate / 100f));
            }

            var pagination = new PaginationResponse(page, totalRows, goodsRes);

            return Result.Ok(pagination);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var discountRate = 100f;
            // if (user.Role != PartnerRole.Default)
            // {
            //     var partnerConfig = _customerManager.GetUserPartnerConfig(user.Role);
            //     discountRate = partnerConfig.DiscountRate / 100f;
            // }

            var goods = await _goodsRepository.Query()
           .Include(e => e.ThumbnailImage)
           .Include(e => e.GoodsMedias).ThenInclude(e => e.Media)
           .Include(e => e.Childrens).ThenInclude(e => e.OptionCombinations).ThenInclude(e => e.Option)
           .Include(e => e.Childrens)
           .Include(e => e.OptionValues).ThenInclude(e => e.Option)
           .FirstOrDefaultAsync(e => e.Id == id);
            if (goods == null) return Result.Fail(ResultCodes.IdInvalid);

            var goodsRes = _mapper.Map<GoodsGetResponse>(goods);

            var authenticationScheme = JwtBearerDefaults.AuthenticationScheme;
            var auth = HttpContext.AuthenticateAsync(authenticationScheme);
            if (auth.IsCompletedSuccessfully)
            {
                var user = await _customerManager.GetUserAsync();
                if (user.Role == PartnerRole.CityPartner && goods.CityDiscount > 0)
                    discountRate = goods.CityDiscount;
                else if (user.Role == PartnerRole.BranchPartner && goods.BranchDiscount > 0)
                    discountRate = goods.BranchDiscount;
            }

            goodsRes.DiscountPrice = (int)(goodsRes.Price * (discountRate / 100f));

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
                DiscountPrice = (int)(s.Price * discountRate),
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
    }
}