using System;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 银行卡
    /// </summary>
    public class BankCard : AggregateRoot
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        [MaxLength(32)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(32)]
        public string BankCode { get; set; }
        [Required]
        [MaxLength(32)]
        public string BankNo { get; set; }
        public DateTime Createat { get; set; }
    }
}