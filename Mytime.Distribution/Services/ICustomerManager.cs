using System.Threading.Tasks;
using Mytime.Distribution.Configs;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface ICustomerManager
    {
        /// <summary>
        /// 获取用户信息和资产
        /// </summary>
        /// <returns></returns>
        Task<Customer> GetUserAndAssetsAsync();
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<Customer> GetUserAsync();
        /// <summary>
        /// 获取用户合伙人角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        PartnerItemConfig GetUserPartnerConfig(PartnerRole role);
        /// <summary>
        /// 修改资产
        /// </summary>
        /// <param name="customerId">顾客Id</param>
        /// <param name="commission">佣金</param>
        /// <param name="amount">可用金额</param>
        Task UpdateAssets(int customerId, int commission, int amount);
    }
}