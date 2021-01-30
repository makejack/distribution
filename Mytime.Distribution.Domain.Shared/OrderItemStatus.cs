namespace Mytime.Distribution.Domain.Shared
{
    public enum OrderItemStatus : byte
    {
        Default = 0,
        /// <summary>
        /// 申请失败
        /// </summary>
        ApplyFaild = 1,

        /// <summary>
        /// 退货申请
        /// </summary>
        RefundApply = 2,

        /// <summary>
        /// 同意退款申请
        /// </summary>
        ConfirmApply = 3,

        /// <summary>
        /// 退还商品
        /// </summary>
        ReturnGoods = 4,

        /// <summary>
        /// 完成退货
        /// </summary>
        CompleteRefund = 5,

        /// <summary>
        /// 等待发货
        /// </summary>
        PendingShipment = 100,

        /// <summary>
        /// 已出货
        /// </summary>
        Shipped = 101,
        
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 200
    }
}