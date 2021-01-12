namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 发货申请通知
    /// </summary>
    public class ShippingApplyNotify : NotifyBase
    {
        /// <summary>
        /// 联系人
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <value></value>
        public string Tel { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            return $"{Header} 您有请的发货申请，收货人：{Name} 联系电话：{Tel} 请及时处理。";
        }
    }
}