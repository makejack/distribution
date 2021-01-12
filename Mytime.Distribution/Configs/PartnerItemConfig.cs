namespace Mytime.Distribution.Configs
{
    /// <summary>
    /// 城市、网点合伙人配置
    /// </summary>
    public class PartnerItemConfig
    {
        /// <summary>
        /// 首次佣金比例
        /// </summary>
        /// <value></value>
        public int FirstCommissionRatio { get; set; }
        /// <summary>
        /// 次级佣金比例
        /// </summary>
        /// <value></value>
        public int SecondaryCommissionRatio { get; set; }
        /// <summary>
        /// 折扣比例
        /// </summary>
        /// <value></value>
        public int DiscountRate { get; set; }
    }
}