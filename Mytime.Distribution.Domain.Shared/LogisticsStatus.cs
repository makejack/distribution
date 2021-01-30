using System.ComponentModel;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 退货物流状态
    /// </summary>
    public enum ReturnLogisticsStatus : byte
    {
        /// <summary>
        /// 已收到
        /// </summary>
        [Description("已收到")]
        Received,
        /// <summary>
        /// 未收到
        /// </summary>
        [Description("未收到")]
        NotReceived
    }
}