using System.Threading.Tasks;
using Mytime.Distribution.Models;
using Mytime.Distribution.Services.Models.Request;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<Result> Cancel(int orderId, string reason);

        /// <summary>
        /// 支付接收
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task PaymentReceived(PaymentReceivedRequest request);
    }
}