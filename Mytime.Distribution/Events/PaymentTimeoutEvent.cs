using MediatR;

namespace Mytime.Distribution.Events
{
    /// <summary>
    /// 支付超时
    /// </summary>
    public class PaymentTimeoutEvent : INotification
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        /// <value></value>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <value></value>
        public string OrderNo { get; set; }
    }
}