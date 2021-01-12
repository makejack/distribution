using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    public class AggregateRoot
    {
        [Key]
        public int Id { get; set; }
    }
}