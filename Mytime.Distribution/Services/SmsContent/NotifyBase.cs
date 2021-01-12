namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 短信通知
    /// </summary>
    public abstract class NotifyBase : INotify
    {
        /// <summary>
        /// 短信内容头部
        /// </summary>
        /// <value></value>
        public string Header { get; set; } = "【蒂脉科技】";

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public abstract string Execute();
    }
}