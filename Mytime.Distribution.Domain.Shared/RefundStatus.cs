using System;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 退款状态
    /// </summary>
    public enum RefundStatus : byte
    {
        Default = 0,
        /// <summary>
        /// 申请失败
        /// </summary>
        ApplyFaild,
        /// <summary>
        /// 退货申请
        /// </summary>
        RefundApply,
        /// <summary>
        /// 同意退款申请
        /// </summary>
        ConfirmApply,
        /// <summary>
        /// 退还商品
        /// </summary>
        ReturnGoods,
        /// <summary>
        /// 完成退货
        /// </summary>
        CompleteRefund,
    }
}