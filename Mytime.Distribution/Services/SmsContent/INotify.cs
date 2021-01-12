namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 短信通知
    /// </summary>
    public interface INotify
    {
        /// <summary>
        /// 短信内容头部
        /// </summary>
        /// <value></value>
        string Header { get; set; }
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        string Execute();
    }
}