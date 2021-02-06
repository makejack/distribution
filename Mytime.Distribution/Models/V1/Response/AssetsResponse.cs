using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 资产响应
    /// </summary>
    public class AssetsResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 总资产 
        /// 总提现金额
        /// </summary>
        /// <value></value>
        public int TotalAssets { get; set; }
        /// <summary>
        /// 可用金额
        /// </summary>
        /// <value></value>
        public int AvailableAmount { get; set; }
        /// <summary>
        /// 总佣金
        /// </summary>
        /// <value></value>
        public int TotalCommission { get; set; }
        /// <summary>
        /// 回购金额
        /// </summary>
        /// <value></value>
        public int RepurchaseAmount { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        /// <value></value>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}