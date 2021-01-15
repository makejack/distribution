using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;
using Mytime.Distribution.Services.Models.Request;
using Mytime.Distribution.Utils.Helpers;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 提现
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/withdrawal")]
    [Produces("application/json")]
    public class WithdrawalController : ControllerBase
    {
        private readonly IRepositoryByInt<WithdrawalHistory> _withdrawalHistoryRepository;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IRepositoryByInt<Assets> _assetsRepository;
        private readonly IPaymentService _paymentService;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="withdrawalHistoryRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="assetsRepository"></param>
        /// <param name="paymentService"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public WithdrawalController(IRepositoryByInt<WithdrawalHistory> withdrawalHistoryRepository,
                                    IRepositoryByInt<Customer> customerRepository,
                                    IRepositoryByInt<Assets> assetsRepository,
                                    IPaymentService paymentService,
                                    ICustomerManager customerManager,
                                    IMapper mapper)
        {
            _withdrawalHistoryRepository = withdrawalHistoryRepository;
            _customerRepository = customerRepository;
            _assetsRepository = assetsRepository;
            _paymentService = paymentService;
            _customerManager = customerManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 提现到银行卡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> PayBank([FromBody] WithdrawalPayBankRequest request)
        {
            var userId = HttpContext.GetUserId();

            var user = await _customerRepository.Query().Include(e => e.Assets).FirstOrDefaultAsync(e => e.Id == userId);
            var assets = user.Assets;
            if (assets == null)
            {
                return Result.Fail(ResultCodes.RequestParamError, "可提现金额不足");
            }

            if (assets.AvailableAmount <= 0) return Result.Fail(ResultCodes.RequestParamError, "可提现金额不足");
            if (assets.AvailableAmount < request.Amount) return Result.Fail(ResultCodes.RequestParamError, "金额不足");
            if (assets.AvailableAmount <= 100000) return Result.Fail(ResultCodes.RequestParamError, "金额不足1000，不可提现");

            var reservedAmount = (int)(request.Amount * 0.2); //保留20%的金额
            var amount = request.Amount - reservedAmount; //提现金额
            var handlingFee = (int)(amount * 0.001); //微信手续费0.1%
            if (handlingFee < 100)
            {
                handlingFee = 100;
            }
            else if (handlingFee > 2500)
            {
                handlingFee = 2500;
            }

            string partnerTradeNo = DateTime.Now.ToString("yyyyMMdd") + GenerateHelper.GenOrderNo();

            var payBankRequest = new PayBankRequest
            {
                PartnerTradeNo = partnerTradeNo,
                Amount = request.Amount,
                BankCode = user.BankCode,
                BankNo = user.BankNo,
                Desc = "提现",
                TrueName = user.Name
            };

            var payBankResult = await _paymentService.PayBank(payBankRequest);

            var withdrawalHistory = new WithdrawalHistory
            {
                CustomerId = user.Id,
                Amount = request.Amount,
                IsSuccess = payBankResult.IsSuccess,
                Message = payBankResult.Message,
                PartnerTradeNo = partnerTradeNo,
                Total = assets.AvailableAmount - amount - handlingFee,
                HandlingFee = handlingFee,
                Createat = DateTime.Now
            };

            _withdrawalHistoryRepository.Insert(withdrawalHistory, false);

            if (payBankResult.IsSuccess)
            {
                assets.AvailableAmount -= (amount + handlingFee);
                assets.TotalAssets = assets.AvailableAmount + assets.TotalCommission;
                assets.UpdateTime = DateTime.Now;

                using (var transaction = _withdrawalHistoryRepository.BeginTransaction())
                {
                    await _withdrawalHistoryRepository.SaveAsync();

                    await _assetsRepository.UpdateAsync(assets);

                    transaction.Commit();
                }
            }

            return Result.Ok(payBankResult);
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("history")]
        public async Task<Result> History([FromQuery] PaginationRequest request)
        {
            var userId = HttpContext.GetUserId();

            var queryable = _withdrawalHistoryRepository.Query().Where(e => e.CustomerId == userId);

            var totalRows = await queryable.CountAsync();
            var historys = await _withdrawalHistoryRepository.Query()
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<WithdrawalHistoryResponse>>(historys)));
        }
    }
}