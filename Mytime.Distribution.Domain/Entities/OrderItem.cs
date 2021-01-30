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
        public OrderItemStatus Status { get; set; }
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
        public DateTime Createat { get; set; }

        public virtual CommissionHistory CommissionHistory { get; set; }
        public virtual List<ShipmentOrderItem> ShipmentOrderItems { get; set; }
        public virtual ReturnApply ReturnApply { get; set; }

        /// <summary>
        /// 设置佣金
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="commission"></param>
        /// <param name="percentage"></param>
        /// <param name="status"></param>
        public void SetCommissionHistory(int? customerId, int commission, int percentage, CommissionStatus status)
        {
            if (!customerId.HasValue || commission == 0) return;
            this.CommissionHistory = new CommissionHistory
            {
                CustomerId = customerId.Value,
                Commission = commission,
                Percentage = percentage,
                Status = status,
                Createat = DateTime.Now
            };
        }
    }
}