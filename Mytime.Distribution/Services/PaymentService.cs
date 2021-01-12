using System;
using System.Threading.Tasks;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.V2;
using Essensoft.AspNetCore.Payment.WeChatPay.V2.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Services.Models.Request;
using Mytime.Distribution.Services.Models.Response;
using Newtonsoft.Json;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 支付服务
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private IHttpContextAccessor _accessor;
        private IWeChatPayClient _client;
        private WeChatPayOptions _wechatPayOption;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="client"></param>
        /// <param name="wechatPayOption"></param>
        public PaymentService(IHttpContextAccessor accessor,
                              IWeChatPayClient client,
                              IOptions<WeChatPayOptions> wechatPayOption)
        {
            _accessor = accessor;
            _client = client;
            _wechatPayOption = wechatPayOption.Value;
        }

        /// <summary>
        /// 提现到银行卡
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PayBankResponse> PayBank(PayBankRequest request)
        {
            var result = new PayBankResponse();

            var wxRequest = new WeChatPayPayBankRequest
            {
                PartnerTradeNo = request.PartnerTradeNo,
                BankNo = request.BankNo,
                TrueName = request.TrueName,
                BankCode = request.BankCode,
                Amount = request.Amount,
                Desc = request.Desc
            };
            try
            {
                var response = await _client.ExecuteAsync(wxRequest, _wechatPayOption);
                if (response.ReturnCode == WeChatPayCode.Success)
                {
                    if (response.ResultCode == WeChatPayCode.Success)
                    {
                        result.IsSuccess = true;
                    }
                    result.Message = response.ErrCodeDes;
                }
                else
                {
                    result.Message = response.ReturnMsg;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 生成支付订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PaymentOrderResponse> GeneratePaymentOrder(GeneratePaymentOrderRequest request)
        {
            var ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var hostUrl = _accessor.HttpContext.Request.GetHostUrl();

            var wxRequest = new WeChatPayUnifiedOrderRequest
            {
                Body = request.Subject,
                OutTradeNo = request.OrderNo,
                TotalFee = request.TotalFee,
                OpenId = request.OpenId,
                TradeType = "JSAPI",
                SpBillCreateIp = ip,
                NotifyUrl = $"{hostUrl}/api/v1/payment/notify/unifiedorder/{request.OrderNo}"
            };

            var response = await _client.ExecuteAsync(wxRequest, _wechatPayOption);
            if (response.ReturnCode == WeChatPayCode.Success && response.ResultCode == WeChatPayCode.Success)
            {
                var req = new WeChatPayMiniProgramSdkRequest
                {
                    Package = $"prepay_id={response.PrepayId}"
                };
                var parameter = await _client.ExecuteAsync(req, _wechatPayOption);

                var json = JsonConvert.SerializeObject(parameter);
                return JsonConvert.DeserializeObject<PaymentOrderResponse>(json);
            }

            throw new Exception(response.ErrCodeDes);
        }
    }
}