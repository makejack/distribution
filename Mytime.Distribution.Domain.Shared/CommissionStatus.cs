using System;
using System.ComponentModel;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 佣金状态
    /// </summary>
    public enum CommissionStatus : byte
    {
        /// <summary>
        /// 待结算
        /// </summary>
        [Description("待结算")]
        PendingSettlement = 0,

        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        Invalidation = 1,

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Complete = 200
    }
}