using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 订单开票
    /// </summary>
    public class OrderBilling : AggregateRoot
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        /// <value></value>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单实体
        /// </summary>
        /// <value></value>
        [ForeignKey(nameof(OrderId))]
        public virtual Orders Orders { get; set; }
        /// <summary>
        /// 是否已开票
        /// </summary>
        /// <value></value>
        public bool IsInvoiced { get; set; }
        /// <summary>
        /// 抬头名称
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string Title { get; set; }
        /// <summary>
        /// 抬头税号
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string TaxNumber { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        [MaxLength(512)]
        public string CompanyAddress { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        [MaxLength(32)]
        public string TelePhone { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        [MaxLength(512)]
        public string BankName { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        [MaxLength(32)]
        public string BankAccount { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [MaxLength(512)]
        public string Email { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}