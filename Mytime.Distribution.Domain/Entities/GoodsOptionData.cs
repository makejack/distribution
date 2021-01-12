using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class GoodsOptionData : AggregateRoot
    {
        public GoodsOptionData()
        {
        }

        public GoodsOptionData(int optionId, string value, string description)
        {
            OptionId = optionId;
            Value = value;
            Description = description;
            Createat = DateTime.Now;
        }

        public int OptionId { get; set; }
        [ForeignKey("OptionId")]
        public virtual GoodsOption GoodsOption { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime Createat { get; set; }
    }
}