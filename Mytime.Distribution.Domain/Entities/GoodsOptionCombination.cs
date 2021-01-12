using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class GoodsOptionCombination : AggregateRoot
    {
        public GoodsOptionCombination()
        {
        }

        public GoodsOptionCombination(int optionId, int displayOrder, string value)
        {
            OptionId = optionId;
            DisplayOrder = displayOrder;
            Value = value;
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
        public int DisplayOrder { get; set; }
        [MaxLength(32)]
        public string Value { get; set; }
        public DateTime Createat { get; set; }
    }
}