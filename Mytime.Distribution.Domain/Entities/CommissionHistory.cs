using System.ComponentModel;
using System;
using Mytime.Distribution.Domain.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 佣金历史记录
    /// </summary>
    public class CommissionHistory : AggregateRoot
    {
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public int? OrderItemId { get; set; }
        [ForeignKey("OrderItemId")]
        public virtual OrderItem OrderItem { get; set; }
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
        public CommissionStatus Status { get; set; }
        public DateTime? SettlementTime { get; set; }
        public DateTime Createat { get; set; }
    }
}