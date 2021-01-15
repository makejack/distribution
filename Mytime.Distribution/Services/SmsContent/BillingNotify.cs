namespace Mytime.Distribution.Services.SmsContent
{

    /// <summary>
    /// 开票通知
    /// </summary>
    public class BillingNotify : NotifyBase
    {
        /// <summary>
        /// 抬头名称
        /// </summary>
        /// <value></value>
        public string Title { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            return $"{Header} 您有新的开票申请，抬头：{Title} 请及时处理。";
        }
    }
}