using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Domain.Entities
{
    public class OrderItem : AggregateRoot
    {
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Orders Order { get; set; }

        public int? GoodsId { get; set; }

        [ForeignKey("GoodsId")]
        public virtual Goods Goods { get; set; }

        /// <summary>
        /// 选择的发货商品Id
        /// </summary>
        /// <value></value>
        public int? GoodsItemId { get; set; }

        [Required]
        [MaxLength(512)]
        public string GoodsName { get; set; }

        public int GoodsPrice { get; set; }

        public int DiscountAmount { get; set; }

        [MaxLength(512)]
        public string GoodsMediaUrl { get; set; }

        [MaxLength(512)]
        public string NormalizedName { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// 是否首批商品
        /// </summary>
        /// <value></value>
        public bool IsFirstBatchGoods { get; set; }

        [MaxLength(512)]
        public string Remarks { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
        public DateTime? ShippingTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 保修期限
        /// </summary>
        /// <value></value>
        public DateTime? WarrantyDeadline { get; set; }
        /*
        /// <summary>
        /// 退货理由
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string ReturnReason { get; set; }
        /// <summary>
        /// 退货时间
        /// </summary>
        /// <value></value>
        public DateTime? ReturnTime { get; set; }
        /// <summary>
        /// 退款费用
        /// </summary>
        /// <value></value>
        public int RefundFee { get; set; }
        */

        public DateTime Createat { get; set; }

        public virtual CommissionHistory CommissionHistory { get; set; }
        public virtual List<ShipmentOrderItem> ShipmentOrderItems { get; set; }
    }
}