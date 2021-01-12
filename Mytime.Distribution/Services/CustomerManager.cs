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
        private readonly PartnerConfig _partnerConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="customerRepository"></param>
        /// <param name="assetsRepository"></param>
        /// <param name="options"></param>
        public CustomerManager(IHttpContextAccessor accessor,
                               IRepositoryByInt<Customer> customerRepository,
                               IRepositoryByInt<Assets> assetsRepository,
                               IOptions<PartnerConfig> options)
        {
            _accessor = accessor;
            _customerRepository = customerRepository;
            _assetsRepository = assetsRepository;
            _partnerConfig = options.Value;
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
        /// <param name="amount">可用金额</param>
        public async Task UpdateAssets(int customerId, int commission, int amount)
        {
            var assets = await _assetsRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == customerId);

            assets.TotalCommission += commission;
            assets.AvailableAmount += amount;
            assets.TotalAssets = assets.TotalCommission + assets.AvailableAmount;
            assets.UpdateTime = DateTime.Now;

            await _assetsRepository.UpdateAsync(assets);
        }

        /// <summary>
        /// 获取用户合伙人角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public PartnerItemConfig GetUserPartnerConfig(PartnerRole role)
        {
            PartnerItemConfig config = null;
            switch (role)
            {
                case PartnerRole.CityPartner:
                    config = _partnerConfig.City;
                    break;
                case PartnerRole.BranchPartner:
                    config = _partnerConfig.Branch;
                    break;
            }
            return config;
        }
    }
}