using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 退货申请
    /// </summary>
    public class ReturnApply : AggregateRoot
    {
        public int OrderItemId { get; set; }
        [ForeignKey(nameof(OrderItemId))]
        public virtual OrderItem OrderItem { get; set; }
        public int ShipmentId { get; set; }
        [ForeignKey(nameof(ShipmentId))]
        public virtual Shipment Shipment { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 退货类型
        /// </summary>
        /// <value></value>
        public ReturnTypes ReturnType { get; set; }
        /// <summary>
        /// 物流状态
        /// </summary>
        /// <value></value>
        public ReturnLogisticsStatus LogisticsStatus { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string Reason { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string Description { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        /// <value></value>
        public int PaymentAmount { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        /// <value></value>
        public int RefundAmount { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        /// <value></value>
        public ReturnAuditStatus Status { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <value></value>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 审核消息
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string AuditMessage { get; set; }
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
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }

        public virtual ReturnAddress ReturnAddress { get; set; }
    }
}