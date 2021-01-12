namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 发货通知
    /// </summary>
    public class ShippingNotify : NotifyBase
    {
        /// <summary>
        /// 快递公司
        /// </summary>
        /// <value></value>
        public string CourierCompany { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        /// <value></value>
        public string TrackingNumber { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            return $"{Header} 尊敬的 {UserName}，您好！您的货物已通过{CourierCompany}快递发货，快递单号是：{TrackingNumber}，请近几天保持电话畅通，以备快递人员及时联系。祝您购物愉快。";
        }
    }
}