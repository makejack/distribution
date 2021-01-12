using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class GoodsOptionValue : AggregateRoot
    {
        public GoodsOptionValue()
        {
        }

        public GoodsOptionValue(int optionId, string value, int displayOrder)
        {
            OptionId = optionId;
            Value = value;
            DisplayOrder = displayOrder;
            Createat = DateTime.Now;
        }

        [Required]
        public int GoodsId { get; set; }
        [ForeignKey("GoodsId")]
        public virtual Goods Goods { get; set; }
        [Required]
        public int OptionId { get; set; }
        [ForeignKey("OptionId")]
        public virtual GoodsOption Option { get; set; }
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime Createat { get; set; }
    }
}