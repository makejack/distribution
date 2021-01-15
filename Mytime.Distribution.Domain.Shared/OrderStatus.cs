namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 交易状态
    /// </summary>
    public enum OrderStatus : byte
    {
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 0,
        /// <summary>
        /// 待付款
        /// </summary>
        PendingPayment,
        /// <summary>
        /// 支付失败
        /// </summary>
        PaymentFailed,
        /// <summary>
        /// 订单关闭
        /// </summary>
        Closed,
        /// <summary>
        /// 已付款
        /// </summary>
        PaymentReceived = 100,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 200,
    }
}