using System.ComponentModel;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 退货审核状态
    /// </summary>
    public enum ReturnAuditStatus : byte
    {
        /// <summary>
        /// 取消申请
        /// </summary>
        [Description("取消申请")]
        Cancel = 0,
        /// <summary>
        /// 审核失败
        /// </summary>
        [Description("审核失败")]
        Failed = 1,
        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        NotAudit = 2,
        /// <summary>
        /// 同意申请
        /// </summary>
        [Description("同意申请")]
        Agree = 3,
        /// <summary>
        /// 退还商品
        /// </summary>
        [Description("退还商品")]
        ReturnGoods = 4,
        /// <summary>
        /// 完成退货
        /// </summary>
        [Description("完成退货")]
        Completed = 5
    }
}