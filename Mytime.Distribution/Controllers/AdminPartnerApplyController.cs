using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台合伙人申请
    /// </summary>
    [Authorize(Roles = "Admin,Goods")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/partner/apply")]
    [Produces("application/json")]
    public class AdminPartnerApplyController : ControllerBase
    {
        private readonly IRepositoryByInt<PartnerApply> _partnerApplyRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="partnerApplyRepository"></param>
        /// <param name="mapper"></param>
        public AdminPartnerApplyController(IRepositoryByInt<PartnerApply> partnerApplyRepository,
                                           IMapper mapper)
        {
            _partnerApplyRepository = partnerApplyRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminPartnerApplyListRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;
            if (page <= 0)
                page = 1;
            if (limit <= 0)
                limit = 10;
            var queryable = _partnerApplyRepository.Query();
            if (request.PartnerRole != null)
            {
                queryable = queryable.Where(e => e.PartnerRole == request.PartnerRole);
            }
            var totalRows = await queryable.CountAsync();
            var partnerApplys = await queryable.OrderByDescending(e => e.Id).Skip((page - 1) * limit).Take(limit).ToListAsync();
            var pagination = new PaginationResponse(page, totalRows, _mapper.Map<List<AdminPartnerApplyListResponse>>(partnerApplys));

            return Result.Ok(pagination);
        }

        /// <summary>
        /// 获取合伙人申请条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var partnerApply = await _partnerApplyRepository.Query()
            .Include(e => e.PartnerApplyGoods).ThenInclude(e => e.Goods).ThenInclude(e => e.ThumbnailImage)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (partnerApply == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminPartnerApplyGetResponse>(partnerApply));
        }

        /// <summary>
        /// 创建合伙人申请条件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] AdminPartnerApplyCreateRequest request)
        {
            var anyPartnerType = _partnerApplyRepository.Query().Any(e => e.PartnerRole == request.PartnerRole);
            if (anyPartnerType) return Result.Fail(ResultCodes.RequestParamError, "当前合伙人类型已设置申请条件");
            if (request.Goods == null || request.Goods.Count == 0) return Result.Fail(ResultCodes.RequestParamError, "请选择商品");
            if (request.TotalAmount == 0) return Result.Fail(ResultCodes.RequestParamError, "总金额不能为0");

            var partnerApply = new PartnerApply
            {
                OriginalPrice = request.OriginalPrice,
                TotalAmount = request.TotalAmount,
                ApplyType = PartnerApplyType.Default,
                PartnerRole = request.PartnerRole,
                TotalQuantity = request.TotalQuantity,
                RepurchaseCommissionRatio = request.ReferralCommissionRatio,
                ReferralCommissionRatio = request.RepurchaseCommissionRatio,
                Createat = DateTime.Now,
            };
            var partnerApplyGoods = new List<PartnerApplyGoods>();
            foreach (var goods in request.Goods)
            {
                partnerApplyGoods.Add(new PartnerApplyGoods(goods.GoodsId, goods.Quantity, goods.Price));
            }
            partnerApply.PartnerApplyGoods = partnerApplyGoods;

            await _partnerApplyRepository.InsertAsync(partnerApply);

            return Result.Ok();
        }

        /// <summary>
        /// 编辑合伙人申请条件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit([FromBody] AdminPartnerApplyEditRequest request)
        {
            var partnerApply = await _partnerApplyRepository.Query()
            .Include(e => e.PartnerApplyGoods)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (partnerApply == null) return Result.Fail(ResultCodes.IdInvalid);
            if (request.Goods == null || request.Goods.Count == 0) return Result.Fail(ResultCodes.RequestParamError, "请选择商品");
            if (request.TotalAmount == 0) return Result.Fail(ResultCodes.RequestParamError, "总金额不能为0");

            if (partnerApply.PartnerRole != request.PartnerRole)
            {
                var anyPartnerType = _partnerApplyRepository.Query().Any(e => e.Id != request.Id && e.PartnerRole == request.PartnerRole);
                return Result.Fail(ResultCodes.RequestParamError, "当前合伙人类型已设置申请条件");
            }

            partnerApply.ReferralCommissionRatio = request.ReferralCommissionRatio;
            partnerApply.RepurchaseCommissionRatio = request.RepurchaseCommissionRatio;
            partnerApply.PartnerRole = request.PartnerRole;
            partnerApply.ApplyType = request.ApplyType;
            partnerApply.TotalQuantity = request.TotalQuantity;
            partnerApply.TotalAmount = request.TotalAmount;
            partnerApply.OriginalPrice = request.OriginalPrice;

            var partnerApplyGoods = new List<PartnerApplyGoods>();
            foreach (var goods in request.Goods)
            {
                var first = partnerApply.PartnerApplyGoods.FirstOrDefault(e => e.GoodsId == goods.GoodsId);
                if (first == null)
                {
                    partnerApplyGoods.Add(new PartnerApplyGoods(goods.GoodsId, goods.Quantity, goods.Price));
                }
                else
                {
                    first.Quantity = goods.Quantity;
                    first.Price = goods.Price;
                    partnerApplyGoods.Add(first);
                }
            }
            partnerApply.PartnerApplyGoods = partnerApplyGoods;

            _partnerApplyRepository.Update(partnerApply, false);

            using (var transaction = _partnerApplyRepository.BeginTransaction())
            {
                await _partnerApplyRepository.SaveAsync();

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 删除合伙人申请条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var partnerApply = await _partnerApplyRepository.FirstOrDefaultAsync(id);
            if (partnerApply == null) return Result.Fail(ResultCodes.IdInvalid);

            await _partnerApplyRepository.RemoveAsync(partnerApply);

            return Result.Ok();
        }
    }
}