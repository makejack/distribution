using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mytime.Distribution.Events;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Handlers
{
    /// <summary>
    /// 短信通知处理
    /// </summary>
    public class SmsNotifyHandler : INotificationHandler<SmsNotifyEvent>
    {

        private readonly ISmsService _smsService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="smsService"></param>
        public SmsNotifyHandler(ISmsService smsService)
        {
            _smsService = smsService;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(SmsNotifyEvent notification, CancellationToken cancellationToken)
        {
            foreach (var item in notification.Tels)
            {
                await _smsService.SendAsync(item, notification.Message);
            }
        }
    }
}