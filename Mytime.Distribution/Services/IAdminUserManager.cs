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
        /// 开票通知
        /// </summary>
        /// <returns></returns>
        Task BillingNotify(INotify notify);

        /// <summary>
        /// 程序异常通知
        /// </summary>
        /// <returns></returns>
        Task ExceptionNotify();
        /// <summary>
        /// 发货申请通知
        /// </summary>
        /// <param name="notify"></param>
        /// <returns></returns>
        Task ShippingApplyNotify(INotify notify);
    }
}