using System;
using System.Threading.Tasks;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.V2;
using Essensoft.AspNetCore.Payment.WeChatPay.V2.Notify;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Events;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 支付通知回调
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/payment/notify")]
    public class PaymentNotifyController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWeChatPayNotifyClient _client;
        private readonly IMediator _mediator;
        private readonly WeChatPayOptions _wechatOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="client"></param>
        /// <param name="mediator"></param>
        /// <param name="wechatOptions"></param>
        public PaymentNotifyController(ILogger<PaymentNotifyController> logger,
                                       IWeChatPayNotifyClient client,
                                       IMediator mediator,
                                       IOptions<WeChatPayOptions> wechatOptions)
        {
            _logger = logger;
            _client = client;
            _mediator = mediator;
            _wechatOptions = wechatOptions.Value;
        }

        /// <summary>
        /// 支付通知回调
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpPost("unifiedorder/{no}")]
        public async Task<IActionResult> Unifiedorder(string no)
        {
            try
            {
                var notify = await _client.ExecuteAsync<WeChatPayUnifiedOrderNotify>(Request, _wechatOptions);
                if (notify.ReturnCode == WeChatPayCode.Success)
                {
                    if (notify.ResultCode == WeChatPayCode.Success)
                    {
                        await _mediator.Publish(new PaymentReceivedEvent
                        {
                            OrderNo = no,
                            PaymentFee = notify.TotalFee,
                            PaymentMethod = PaymentMethods.Wechat,
                            PaymentTime = DateTime.ParseExact(notify.TimeEnd, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture)
                        });
                        return WeChatPayNotifyResult.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return WeChatPayNotifyResult.Failure;
        }
    }
}