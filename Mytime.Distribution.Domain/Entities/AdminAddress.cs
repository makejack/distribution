using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 后台地址
    /// </summary>
    
    public class AdminAddress : AggregateRoot
    {
        /// <summary>
        /// 是否默认地址
        /// </summary>
        /// <value></value>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        /// <value></value>
        public int PostalCode { get; set; }
        public int ProvinceCode { get; set; }
        [ForeignKey("ProvinceCode")]
        public virtual Province Province { get; set; }
        public int CityCode { get; set; }
        [ForeignKey("CityCode")]
        public virtual City City { get; set; }
        public int AreaCode { get; set; }
        [ForeignKey("AreaCode")]
        public virtual Area Area { get; set; }
        [Required]
        [MaxLength(512)]
        public string DetailInfo { get; set; }
        [Required]
        [MaxLength(32)]
        public string TelNumber { get; set; }
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }
        public DateTime Createat { get; set; }
    }
}