using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class City
    {
        [Key]
        public int Code { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public int? ProvinceCode { get; set; }
        [ForeignKey("ProvinceCode")]
        public virtual Province Province { get; set; }
    }
}