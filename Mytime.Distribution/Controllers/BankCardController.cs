using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Entities;
using AutoMapper;
using System.Threading.Tasks;
using Mytime.Distribution.Models;
using Mytime.Distribution.Extensions;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Models.V1.Response;
using System.Text.RegularExpressions;
using Mytime.Distribution.Utils.Helpers;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 银行卡
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/bankcard")]
    [Produces("application/json")]
    public class BankCardController : ControllerBase
    {
        private readonly IRepositoryByInt<BankCard> _bankCardRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bankCardRepository"></param>
        /// <param name="mapper"></param>
        public BankCardController(IRepositoryByInt<BankCard> bankCardRepository, IMapper mapper)
        {
            _bankCardRepository = bankCardRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取银行卡列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(List<BankCardResponse>), 200)]
        public async Task<Result> List()
        {
            var userId = HttpContext.GetUserId();

            var bankCards = await _bankCardRepository.Query().Where(e => e.CustomerId == userId).ToListAsync();
            var responses = _mapper.Map<List<BankCardResponse>>(bankCards);
            foreach (var item in responses)
            {
                item.BankNo = Regex.Replace(item.BankNo, @"(\w{4})\d{10}(\w{4})", "$1*****$2");
                item.BankName = BankHelper.GetBankName(item.BankCode);
            }

            return Result.Ok(responses);
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] BankCardCreateRequest request)
        {
            var userId = HttpContext.GetUserId();

            var bankCard = new BankCard
            {
                CustomerId = userId,
                BankCode = request.BankCode,
                BankNo = request.BankNo,
                Createat = DateTime.Now,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            await _bankCardRepository.InsertAsync(bankCard);

            return Result.Ok();
        }

        /// <summary>
        /// 删除银行卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var bankCard = await _bankCardRepository.FirstOrDefaultAsync(id);
            if (bankCard == null) return Result.Fail(ResultCodes.IdInvalid);

            await _bankCardRepository.RemoveAsync(bankCard);

            return Result.Ok();
        }
    }
}