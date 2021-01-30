using System.Threading.Tasks;
using Mytime.Distribution.Services.SmsContent;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 后台用户管理服务
    /// </summary>
    public interface IAdminUserManager
    {
        /// <summary>
        /// 账务通知
        /// </summary>
        /// <returns></returns>
        Task AccountingNotify(INotify notify);

        /// <summary>
        /// 开发人员通知
        /// </summary>
        /// <returns></returns>
        Task DevelopmentNotify();
        /// <summary>
        /// 仓库通知
        /// </summary>
        /// <param name="notify"></param>
        /// <returns></returns>
        Task WarehouseNotify(INotify notify);
    }
}