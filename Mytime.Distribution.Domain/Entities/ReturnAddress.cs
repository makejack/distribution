using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 退货地址
    /// </summary>
    public class ReturnAddress : AggregateRoot
    {
        public int ReturnApplyId { get; set; }
        [ForeignKey("ReturnApplyId")]
        public virtual ReturnApply ReturnApply { get; set; }
        [Required]
        [MaxLength(32)]
        public string PostalCode { get; set; }
        [Required]
        [MaxLength(512)]
        public string ProvinceName { get; set; }
        [Required]
        [MaxLength(512)]
        public string CityName { get; set; }
        [Required]
        [MaxLength(512)]
        public string AreaName { get; set; }
        [Required]
        [MaxLength(512)]
        public string DetailInfo { get; set; }
        [Required]
        [MaxLength(32)]
        public string TelNumber { get; set; }
        [Required]
        [MaxLength(32)]
        public string UserName { get; set; }
        [MaxLength(512)]
        public string Remarks { get; set; }
        public DateTime Createat { get; set; }
    }
}