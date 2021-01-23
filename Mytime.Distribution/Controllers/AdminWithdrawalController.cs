using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.V2;
using Essensoft.AspNetCore.Payment.WeChatPay.V2.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台提现
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/withdrawal")]
    [Produces("application/json")]
    public class AdminWithdrawalController : ControllerBase
    {
        private readonly IWeChatPayClient _client;
        private readonly WeChatPayOptions _weChatPayOptions;
        private readonly IRepositoryByInt<WithdrawalHistory> _withdrawalHistoryRepository;
        private readonly IRepositoryByInt<Customer> _custoemrRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <param name="withdrawalHistoryRepository"></param>
        /// <param name="custoemrRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public AdminWithdrawalController(IWeChatPayClient client,
                                         IOptions<WeChatPayOptions> options,
                                         IRepositoryByInt<WithdrawalHistory> withdrawalHistoryRepository,
                                         IRepositoryByInt<Customer> custoemrRepository,
                                         ICustomerManager customerManager,
                                         IMapper mapper)
        {
            _client = client;
            _weChatPayOptions = options.Value;
            _withdrawalHistoryRepository = withdrawalHistoryRepository;
            _custoemrRepository = custoemrRepository;
            _customerManager = customerManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        public async Task<Result> History([FromQuery] AdminWithdrawalHistoryRequest request)
        {
            var queryable = _withdrawalHistoryRepository.Query();

            if (request.CustomerId.HasValue)
            {
                queryable = queryable.Where(e => e.CustomerId == request.CustomerId.Value);
            }

            var totalRows = await queryable.CountAsync();
            var histores = await queryable
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<WithdrawalHistoryResponse>>(histores)));
        }

        /// <summary>
        /// 提现申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("applylist")]
        public async Task<Result> ApplyList([FromQuery] PaginationRequest request)
        {
            var queryable = _withdrawalHistoryRepository.Query().Where(e => e.Status == WithdrawalStatus.Apply);

            var totalRows = await queryable.CountAsync();
            var histores = await queryable.Include(e => e.Customer)
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminWithdrawalHistoryResponse>>(histores)));
        }

        /// <summary>
        /// 确认申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("agree/{id}")]
        public async Task<Result> Agree(int id)
        {
            var apply = await _withdrawalHistoryRepository.FirstOrDefaultAsync(id);
            if (apply == null) return Result.Fail(ResultCodes.IdInvalid);
            if (apply.Status != WithdrawalStatus.Apply) return Result.Fail(ResultCodes.RequestParamError, "当前申请状态不允许确认");

            var openId = await _custoemrRepository.Query()
            .Where(e => e.Id == apply.CustomerId)
            .Select(e => e.OpenId)
            .FirstOrDefaultAsync();

            apply.Status = WithdrawalStatus.Success;
            apply.Message = "提现成功";

            var wxRequest = new WeChatPayPromotionTransfersRequest
            {
                PartnerTradeNo = apply.PartnerTradeNo,
                OpenId = openId,
                CheckName = "FORCE_CHECK",//  "NO_CHECK"
                ReUserName = apply.Name,
                Amount = apply.Amount,
                Desc = "提现"
            };
            var response = await _client.ExecuteAsync(wxRequest, _weChatPayOptions);
            if (response.ReturnCode != WeChatPayCode.Success || response.ResultCode != WeChatPayCode.Success)
            {
                apply.Status = WithdrawalStatus.Failed;
                apply.Message = response.ErrCodeDes;

                var amount = apply.Amount + apply.HandlingFee;
                await _customerManager.UpdateAssets(apply.CustomerId, 0, amount, "提现失败返回金额");
            }

            await _withdrawalHistoryRepository.UpdateAsync(apply);

            return Result.Ok();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("cancel")]
        public async Task<Result> Cancel([FromBody] AdminWithdrawalCancelRequest request)
        {
            var apply = await _withdrawalHistoryRepository.FirstOrDefaultAsync(request.Id);
            if (apply == null) return Result.Fail(ResultCodes.IdInvalid);
            if (apply.Status != WithdrawalStatus.Apply) return Result.Fail(ResultCodes.RequestParamError, "当前申请状态不允许确认");

            apply.Status = WithdrawalStatus.Failed;
            apply.Message = request.Message;

            _withdrawalHistoryRepository.Update(apply, false);

            var amount = apply.Amount + apply.HandlingFee;
            using (var transaction = _withdrawalHistoryRepository.BeginTransaction())
            {
                await _withdrawalHistoryRepository.SaveAsync();

                await _customerManager.UpdateAssets(apply.CustomerId, 0, amount, "提现失败返回金额");

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] PaginationRequest request)
        {
            var queryable = _withdrawalHistoryRepository.Query().Where(e => e.Status != WithdrawalStatus.Apply);

            var totalRows = await queryable.CountAsync();
            var histores = await queryable.Include(e => e.Customer)
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminWithdrawalHistoryResponse>>(histores)));
        }
    }
}