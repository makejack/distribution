using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 资产历史记录
    /// </summary>
    public class AssetsHistory : AggregateRoot
    {
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 总金额（余额）
        /// </summary>
        /// <value></value>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 消费
        /// </summary>
        /// <value></value>
        public int Amount { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string Message { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}