using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    public class GoodsOption : AggregateRoot
    {
        public GoodsOption()
        {
        }

        public GoodsOption(string name)
        {
            Name = name;
            Createat = DateTime.Now;
        }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        public DateTime Createat { get; set; }

        public virtual List<GoodsOptionData> GoodsOptionDatas { get; set; }
        public virtual List<GoodsOptionCombination> GoodsOptionCombinations { get; set; }
        public virtual List<GoodsOptionValue> GoodsOptionValues { get; set; }
    }
}