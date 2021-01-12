namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 发送状态
    /// </summary>
    public enum ShippingStatus : byte
    {
        Default = 0,
        /// <summary>
        /// 申请失败
        /// </summary>
        ApplyFaild,
        /// <summary>
        /// 退货申请
        /// </summary>
        ReturnApply,
        /// <summary>
        /// 确认申请
        /// </summary>
        ConfirmApply,
        /// <summary>
        /// 退还商品
        /// </summary>
        ReturnGoods,
        /// <summary>
        /// 完成退货
        /// </summary>
        CompleteReturn,
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