using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class Area
    {
        [Key]
        public int Code { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public int? CityCode { get; set; }
        [ForeignKey("CityCode")]
        public virtual City City { get; set; }
        public int? ProvinceCode { get; set; }
        [ForeignKey("ProvinceCode")]
        public virtual Province Province { get; set; }
    }
}