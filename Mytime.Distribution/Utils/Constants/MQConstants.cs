namespace Mytime.Distribution.Utils.Constants
{
    /// <summary>
    /// RabbitMQ 常量
    /// </summary>
    public class MQConstants
    {
        /// <summary>
        /// 路由器名称
        /// </summary>
        public const string Exchange = "Distribution";
        /// <summary>
        /// 支付超时Key
        /// </summary>
        public const string PaymentTimeOutRouteKey = "PaymentTimeOut";
        /// <summary>
        /// 自动收货Key
        /// </summary>
        public const string AutoReceivedShippingKey = "AutoReceivedShipping";
    }
}