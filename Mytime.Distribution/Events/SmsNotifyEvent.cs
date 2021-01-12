using MediatR;
using Mytime.Distribution.Services.SmsContent;
using System.Collections.Generic;

namespace Mytime.Distribution.Events
{
    /// <summary>
    /// 短信通知
    /// </summary>
    public class SmsNotifyEvent : INotification
    {
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public IEnumerable<string> Tels { get; set; }
        /// <summary>
        /// 发送的信息
        /// </summary>
        /// <value></value>
        public INotify Message { get; set; }
    }
}