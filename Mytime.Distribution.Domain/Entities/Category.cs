using System;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    public class Category : AggregateRoot
    {
        public Category()
        {
        }

        public Category(string name, int sort)
        {
            Name = name;
            Sort = sort;
            Createat = DateTime.Now;
        }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        public int Sort { get; set; }
        public DateTime Createat { get; set; }
    }
}