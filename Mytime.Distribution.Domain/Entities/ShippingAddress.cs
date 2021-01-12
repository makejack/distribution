using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
namespace Mytime.Distribution.Domain.Entities
{
    public class ShippingAddress : AggregateRoot
    {
        public int ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
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
        public DateTime Createat { get; set; }
    }
}