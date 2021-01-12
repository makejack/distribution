namespace Mytime.Distribution.Configs
{
    /// <summary>
    /// 合伙人配置
    /// </summary>
    public class PartnerConfig
    {
        /// <summary>
        /// 城市合伙人
        /// </summary>
        /// <value></value>
        public PartnerItemConfig City { get; set; }
        /// <summary>
        /// 网点合伙人
        /// </summary>
        /// <value></value>
        public PartnerItemConfig Branch { get; set; }
    }
}