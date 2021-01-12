using System;
using System.ComponentModel;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 合伙人类型
    /// </summary>
    public enum PartnerRole : byte
    {
        Default = 0,
        /// <summary>
        /// 城市合伙人
        /// </summary>
        [Description("城市合伙人")]
        CityPartner = 1,
        /// <summary>
        /// 网点合伙人
        /// </summary>
        [Description("网点合伙人")]
        BranchPartner = 2
    }
}
