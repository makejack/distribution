using System.Threading.Tasks;
using Mytime.Distribution.Services.Models.Request;
using Mytime.Distribution.Services.Models.Response;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 支付服务
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// 生成支付订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaymentOrderResponse> GeneratePaymentOrder(GeneratePaymentOrderRequest request);
        /// <summary>
        /// 提现到银行卡
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PayBankResponse> PayBank(PayBankRequest request);
    }
}