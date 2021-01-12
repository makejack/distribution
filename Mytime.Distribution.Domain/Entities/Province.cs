using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    public class Province
    {
        [Key]
        public int Code { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
    }
}