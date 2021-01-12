using System.ComponentModel;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 合伙人申请类型
    /// </summary>
    public enum PartnerApplyType : byte
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0
    }
}