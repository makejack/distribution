namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 退货通知
    /// </summary>
    public class ReturnNotify : NotifyBase
    {
        /// <summary>
        /// 买家名称
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            return $"{Header} 买家：{UserName} 申请退货，请及时处理。";
        }
    }
}