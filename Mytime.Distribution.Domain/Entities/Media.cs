using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    public class Media : AggregateRoot
    {
        public Media()
        {
        }

        public Media(string url, string type, long size)
        {
            Url = url.Replace(@"\", "/");
            Type = type;
            Size = size;
            Createat = DateTime.Now;
        }

        [Required]
        [MaxLength(512)]
        public string Url { get; set; }
        [Required]
        [MaxLength(32)]
        public string Type { get; set; }
        public long Size { get; set; }
        public DateTime Createat { get; set; }

    }
}