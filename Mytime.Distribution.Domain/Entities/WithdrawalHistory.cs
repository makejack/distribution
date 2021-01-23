using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 提现历史记录
    /// </summary>
    public class WithdrawalHistory : AggregateRoot
    {
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string BankCode { get; set; }
        [MaxLength(32)]
        public string BankNo { get; set; }
        [Required]
        [MaxLength(32)]
        public string PartnerTradeNo { get; set; }
        public int Total { get; set; }
        public int Amount { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        /// <value></value>
        public int HandlingFee { get; set; }
        public WithdrawalStatus Status { get; set; }
        [MaxLength(512)]
        public string Message { get; set; }
        public DateTime Createat { get; set; }
    }
}