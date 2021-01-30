using System.Text.RegularExpressions;
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
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Services.SmsContent;

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
        private readonly IAdminUserManager _adminUserManager;
        private readonly IRepositoryByInt<BankCard> _bankCardRepository;
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
        /// <param name="bankCardRepository"></param>
        /// <param name="adminUserManager"></param>
        /// <param name="customerRepository"></param>
        /// <param name="assetsRepository"></param>
        /// <param name="paymentService"></param>
        /// <param name="customerManager"></param>
        /// <param name="mapper"></param>
        public WithdrawalController(IAdminUserManager adminUserManager,
                                    IRepositoryByInt<BankCard> bankCardRepository,
                                    IRepositoryByInt<WithdrawalHistory> withdrawalHistoryRepository,
                                    IRepositoryByInt<Customer> customerRepository,
                                    IRepositoryByInt<Assets> assetsRepository,
                                    IPaymentService paymentService,
                                    ICustomerManager customerManager,
                                    IMapper mapper)
        {
            _adminUserManager = adminUserManager;
            _bankCardRepository = bankCardRepository;
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
            var bankCard = await _bankCardRepository.FirstOrDefaultAsync(request.Id);
            // var user = await _customerRepository.Query().Include(e => e.Assets).FirstOrDefaultAsync(e => e.Id == userId);
            var assets = await _assetsRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == userId);
            if (assets == null)
            {
                return Result.Fail(ResultCodes.RequestParamError, "可提现金额不足");
            }
            if (assets.AvailableAmount <= 0) return Result.Fail(ResultCodes.RequestParamError, "可提现金额不足");
            if (assets.AvailableAmount < request.Amount) return Result.Fail(ResultCodes.RequestParamError, "金额不足");
            if (request.Amount < 100000) return Result.Fail(ResultCodes.RequestParamError, "金额不足1000，不可提现");

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
                BankCode = bankCard.BankCode,
                BankNo = bankCard.BankNo,
                Desc = "提现",
                TrueName = bankCard.Name
            };

            var payBankResult = await _paymentService.PayBank(payBankRequest);

            var withdrawalHistory = new WithdrawalHistory
            {
                CustomerId = bankCard.CustomerId,
                Name = bankCard.Name,
                BankCode = bankCard.BankCode,
                BankNo = bankCard.BankNo,
                Amount = amount,
                Status = payBankResult.IsSuccess ? WithdrawalStatus.Success : WithdrawalStatus.Failed,
                Message = payBankResult.Message,
                PartnerTradeNo = partnerTradeNo,
                Total = request.Amount,
                HandlingFee = handlingFee,
                Createat = DateTime.Now
            };

            _withdrawalHistoryRepository.Insert(withdrawalHistory, false);

            using (var transaction = _withdrawalHistoryRepository.BeginTransaction())
            {
                await _withdrawalHistoryRepository.SaveAsync();

                if (payBankResult.IsSuccess)
                {
                    await _customerManager.UpdateAssets(assets, -(amount + handlingFee), "提现");
                }
                transaction.Commit();
            }

            return Result.Ok(payBankResult);
        }

        /// <summary>
        /// 申请
        /// </summary>
        /// <returns></returns>
        [HttpPost("apply")]
        public async Task<Result> Apply([FromBody] WithdrawalApplyRequest request)
        {
            var userId = HttpContext.GetUserId();
            var user = await _customerManager.GetUserAndAssetsAsync();
            var assets = user.Assets;
            if (assets == null)
            {
                return Result.Fail(ResultCodes.RequestParamError, "可提现金额不足");
            }
            if (assets.AvailableAmount <= 0) return Result.Fail(ResultCodes.RequestParamError, "可提现金额不足");
            if (assets.AvailableAmount < request.Amount) return Result.Fail(ResultCodes.RequestParamError, "金额不足");
            if (request.Amount < 100000) return Result.Fail(ResultCodes.RequestParamError, "金额不足1000，不可提现");
            if (string.IsNullOrEmpty(user.Name)) return Result.Fail(ResultCodes.RequestParamError, "姓名不能为空");

            var anyApply = await _withdrawalHistoryRepository.Query().AnyAsync(e => e.CustomerId == userId && e.Status == WithdrawalStatus.Apply);
            if (anyApply) return Result.Fail(ResultCodes.RequestParamError, "提现申请已提交，不允许重复申请。");

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
            var withdrawalHistory = new WithdrawalHistory
            {
                CustomerId = user.Id,
                Name = user.Name,
                BankCode = string.Empty,
                BankNo = string.Empty,
                Amount = amount,
                Status = WithdrawalStatus.Apply,
                Message = string.Empty,
                PartnerTradeNo = partnerTradeNo,
                Total = request.Amount,
                HandlingFee = handlingFee,
                Createat = DateTime.Now
            };

            _withdrawalHistoryRepository.Insert(withdrawalHistory, false);

            using (var transaction = _withdrawalHistoryRepository.BeginTransaction())
            {
                await _withdrawalHistoryRepository.SaveAsync();

                await _customerManager.UpdateAssets(assets, -(amount + handlingFee), "提现");

                transaction.Commit();
            }

            await _adminUserManager.AccountingNotify(new WithdrawalNotify
            {
                UserName = user.Name,
                Amount = amount
            });

            return Result.Ok();
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("history")]
        [ProducesResponseType(typeof(List<WithdrawalHistoryResponse>), 200)]
        public async Task<Result> History([FromQuery] PaginationRequest request)
        {
            var userId = HttpContext.GetUserId();

            var queryable = _withdrawalHistoryRepository.Query().Where(e => e.CustomerId == userId);

            var totalRows = await queryable.CountAsync();
            var historys = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<WithdrawalHistoryResponse>>(historys)));
        }
    }
}