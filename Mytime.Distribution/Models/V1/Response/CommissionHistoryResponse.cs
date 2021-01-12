using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 佣金历史记录响应
    /// </summary>
    public class CommissionHistoryResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        /// <value></value>
        public int Commission { get; set; }
        /// <summary>
        /// 比例
        /// </summary>
        /// <value></value>
        public int Percentage { get; set; }
        /// <summary>
        /// 佣金状态
        /// </summary>
        /// <value></value>
        public CommissionStatus Status { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        /// <value></value>
        public DateTime? SettlementTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}