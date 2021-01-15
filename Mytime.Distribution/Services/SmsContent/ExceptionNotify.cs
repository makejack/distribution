namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 异常通知
    /// </summary>
    public class ExceptionNotify : NotifyBase
    {

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            return $"{Header} 程序发生异常，请及时处理。";
        }
    }
}