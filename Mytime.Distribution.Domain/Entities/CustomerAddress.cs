using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    public class CustomerAddress : AggregateRoot
    {
        public CustomerAddress()
        {
        }

        public CustomerAddress(int customerId,
        bool isDefault,
                               int postalCode,
                               int provinceCode,
                               int cityCode,
                               int areaCode,
                               string detailInfo,
                               string telNumber,
                               string userName)
        {
            CustomerId = customerId;
            IsDefault = isDefault;
            PostalCode = postalCode;
            ProvinceCode = provinceCode;
            CityCode = cityCode;
            AreaCode = areaCode;
            DetailInfo = detailInfo;
            TelNumber = telNumber;
            UserName = userName;
            Createat = DateTime.Now;
        }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
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