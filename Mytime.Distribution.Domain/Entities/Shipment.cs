using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 装货数据
    /// </summary>
    public class Shipment : AggregateRoot
    {
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        /// <value></value>
        public int TotalWeight { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
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
        [MaxLength(512)]
        public string CourierCompanyCode { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        /// <value></value>
        [MaxLength(32)]
        public string TrackingNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string Remarks { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public DateTime? ShippingTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public DateTime Createat { get; set; }

        public virtual List<ShipmentOrderItem> ShipmentOrderItems { get; set; }

        public virtual ShippingAddress ShippingAddress { get; set; }
    }
}