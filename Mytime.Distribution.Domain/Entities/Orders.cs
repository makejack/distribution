using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mytime.Distribution.Domain.Shared;
using System;

namespace Mytime.Distribution.Domain.Entities
{
    public class Orders : AggregateRoot
    {
        public Orders()
        {
        }

        public Orders(long orderNO)
        {
            OrderNo = DateTime.Now.ToString("yyyyMMdd") + orderNO;
        }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [MaxLength(32)]
        public string OrderNo { get; set; }
        public int TotalFee { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        /// <value></value>
        public int ActuallyAmount { get; set; }
        /// <summary>
        /// 钱包金额
        /// </summary>
        /// <value></value>
        public int WalletAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentTypes PaymentType { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public int PaymentFee { get; set; }
        public DateTime? PaymentTime { get; set; }
        /// <summary>
        /// 退款理由
        /// </summary>
        /// <value></value>
        [MaxLength(512)]
        public string RefundReason { get; set; }
        public DateTime? RefundTime { get; set; }
        public int RefundFee { get; set; }
        [MaxLength(512)]
        public string Remarks { get; set; }
        public string CancelReason { get; set; }
        public DateTime? CancelTime { get; set; }
        public DateTime Createat { get; set; }
        /// <summary>
        /// 扩展参数
        /// </summary>
        /// <value></value>
        public string ExtendParams { get; set; }
        /// <summary>
        /// 是否第一笔订单
        /// </summary>
        /// <value></value>
        public bool IsFistOrder { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
        public virtual OrderBilling OrderBilling { get; set; }
    }
}