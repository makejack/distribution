using System.ComponentModel;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Configs;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Extensions;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class CustomerManager : ICustomerManager
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IRepositoryByInt<Assets> _assetsRepository;
        private readonly IRepositoryByInt<AssetsHistory> _assetsHistoryRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="customerRepository"></param>
        /// <param name="assetsRepository"></param>
        /// <param name="assetsHistoryRepository"></param>
        public CustomerManager(IHttpContextAccessor accessor,
                               IRepositoryByInt<Customer> customerRepository,
                               IRepositoryByInt<Assets> assetsRepository,
                               IRepositoryByInt<AssetsHistory> assetsHistoryRepository)
        {
            _accessor = accessor;
            _customerRepository = customerRepository;
            _assetsRepository = assetsRepository;
            _assetsHistoryRepository = assetsHistoryRepository;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public Task<Customer> GetUserAsync()
        {
            var userId = _accessor.HttpContext.GetUserId();

            return _customerRepository.FirstOrDefaultAsync(userId);
        }

        /// <summary>
        /// 获取用户和父级信息
        /// </summary>
        /// <returns></returns>
        public Task<Customer> GetUserAndParentAsync()
        {
            var userId = _accessor.HttpContext.GetUserId();

            return _customerRepository.Query().Include(e => e.Parent).FirstOrDefaultAsync(e => e.Id == userId);
        }

        /// <summary>
        /// 获取用户信息和资产
        /// </summary>
        /// <returns></returns>
        public Task<Customer> GetUserAndAssetsAsync()
        {
            var userId = _accessor.HttpContext.GetUserId();

            return _customerRepository.Query().Include(e => e.Assets).FirstOrDefaultAsync(e => e.Id == userId);
        }

        /// <summary>
        /// 修改资产
        /// </summary>
        /// <param name="customerId">顾客Id</param>
        /// <param name="commission">佣金</param>
        public async Task UpdateAssets(int customerId, int commission)
        {
            var assets = await _assetsRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == customerId);

            assets.TotalCommission += commission;
            assets.TotalAssets = assets.TotalCommission + assets.AvailableAmount;
            assets.UpdateTime = DateTime.Now;

            await _assetsRepository.UpdateAsync(assets);
        }

        /// <summary>
        /// 修改资产
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="commission"></param>
        /// <param name="amount"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task UpdateAssets(int customerId, int commission, int amount, string message)
        {
            var assets = await _assetsRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == customerId);

            assets.TotalCommission += commission;
            assets.AvailableAmount += amount;
            assets.TotalAssets = assets.TotalCommission + assets.AvailableAmount;
            assets.UpdateTime = DateTime.Now;

            await _assetsRepository.UpdateAsync(assets);

            if (amount != 0)
            {
                var history = new AssetsHistory
                {
                    CustomerId = customerId,
                    Amount = amount,
                    TotalAmount = assets.AvailableAmount,
                    Createat = DateTime.Now,
                    Message = message
                };

                await _assetsHistoryRepository.InsertAsync(history);
            }
        }

        /// <summary>
        /// 修改资产
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="amount"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task UpdateAssets(Assets assets, int amount, string message)
        {
            assets.AvailableAmount += amount;
            assets.TotalAssets = assets.TotalCommission + assets.AvailableAmount;
            assets.UpdateTime = DateTime.Now;

            await _assetsRepository.UpdateAsync(assets);

            var history = new AssetsHistory
            {
                CustomerId = assets.CustomerId,
                TotalAmount = assets.AvailableAmount,
                Amount = amount,
                Createat = DateTime.Now,
                Message = message
            };

            await _assetsHistoryRepository.InsertAsync(history);
        }
    }
}