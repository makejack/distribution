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
        /// 获取用户和父级信息
        /// </summary>
        /// <returns></returns>
        Task<Customer> GetUserAndParentAsync();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<Customer> GetUserAsync();
        /// <summary>
        /// 修改资产
        /// </summary>
        /// <param name="customerId">顾客Id</param>
        /// <param name="commission">佣金</param>
        Task UpdateAssets(int customerId, int commission);
        /// <summary>
        /// 修改资产
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="amount"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task UpdateAssets(Assets assets, int amount, string message);
        /// <summary>
        /// 修改资产
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="commission"></param>
        /// <param name="amount"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task UpdateAssets(int customerId, int commission, int amount, string message);
    }
}