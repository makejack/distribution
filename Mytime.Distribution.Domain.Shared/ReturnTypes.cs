using System.ComponentModel;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 退货类型
    /// </summary>
    public enum ReturnTypes : byte
    {
        /// <summary>
        /// 退货退款
        /// </summary>
        [Description("退货退款")]
        Return,
        /// <summary>
        /// 退货退款
        /// </summary>
        [Description("换货")]
        Exchange
    }
}