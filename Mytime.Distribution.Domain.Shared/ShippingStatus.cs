namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 发送状态
    /// </summary>
    public enum ShippingStatus : byte
    {
        Default = 0,
        /// <summary>
        /// 退还商品
        /// </summary>
        ReturnGoods,
        /// <summary>
        /// 等待发货
        /// </summary>
        PendingShipment = 100,
        /// <summary>
        /// 已出货
        /// </summary>
        Shipped,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 200
    }
}