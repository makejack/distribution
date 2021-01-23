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
        /// <summary>
        /// 发货状态
        /// </summary>
        /// <value></value>
        public ShippingStatus ShippingStatus { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        /// <value></value>
        public DateTime? ShippingTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <value></value>
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 保修期限
        /// </summary>
        /// <value></value>
        public DateTime? WarrantyDeadline { get; set; }
        /// <summary>
        /// 退款状态
        /// </summary>
        /// <value></value>
        public RefundStatus RefundStatus { get; set; }
        /// <summary>
        /// 退货理由
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string RefundReason { get; set; }
        /// <summary>
        /// 退货申请时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundApplyTime { get; set; }
        /// <summary>
        /// 退货时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        /// <value></value>
        public int RefundAmount { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string CourierCompany { get; set; }
        /// <summary>
        /// 快递公司Code
        /// </summary>
        /// <value></value>
        [MaxLength(128)]
        public string CourierCompanyCode { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        /// <value></value>
        [MaxLength(32)]
        public string TrackingNumber { get; set; }
        public DateTime Createat { get; set; }

        public virtual CommissionHistory CommissionHistory { get; set; }
        public virtual List<ShipmentOrderItem> ShipmentOrderItems { get; set; }
        public virtual ReturnAddress ReturnAddress { get; set; }
    }
}