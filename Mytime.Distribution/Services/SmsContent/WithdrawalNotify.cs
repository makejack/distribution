using System.Reflection.Metadata.Ecma335;
namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 提现通知
    /// </summary>
    public class WithdrawalNotify : NotifyBase
    {
        /// <summary>
        /// 提现人员名称
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        /// <value></value>
        public int Amount { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            return $"{Header} 用户：{UserName} 申请提现，提现金额：{Amount / 100} ，请及时处理。";
        }
    }
}