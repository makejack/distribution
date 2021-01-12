using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class PartnerApplyGoods : AggregateRoot
    {
        public PartnerApplyGoods()
        {
        }

        public PartnerApplyGoods(int goodsId, int quantity, int price)
        {
            GoodsId = goodsId;
            Quantity = quantity;
            Price = price;
            Createat = DateTime.Now;
        }

        public int PartnerApplyId { get; set; }
        [ForeignKey("PartnerApplyId")]
        public virtual PartnerApply PartnerApply { get; set; }
        public int GoodsId { get; set; }
        [ForeignKey("GoodsId")]
        public virtual Goods Goods { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime Createat { get; set; }
    }
}