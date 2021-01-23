using System.Collections.Generic;
using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取订单详情
    /// </summary>
    public class AdminOrderGetResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <value></value>
        public string OrderNo { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        /// <value></value>
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
        /// <summary>
        /// 订单状态
        /// </summary>
        /// <value></value>
        public OrderStatus OrderStatus { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        /// <value></value>
        public PaymentTypes PaymentType { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <value></value>
        public PaymentMethods PaymentMethod { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        /// <value></value>
        public int PaymentFee { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        /// <value></value>
        public DateTime? PaymentTime { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        /// <value></value>
        public string RefundReason { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        /// <value></value>
        public int RefundFee { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        /// <value></value>
        public string CancelReason { get; set; }
        /// <summary>
        /// 取消时间
        /// </summary>
        /// <value></value>
        public DateTime? CancelTime { get; set; }
        /// <summary>
        /// 扩展参数
        /// </summary>
        /// <value></value>
        public string ExtendParams { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }

        /// <summary>
        /// 订单商品
        /// </summary>
        /// <value></value>
        public List<AdminOrderItemResponse> Items { get; set; }
        /// <summary>
        /// 票据
        /// </summary>
        /// <value></value>
        public AdminOrderBillingResponse Billing { get; set; }
    }
}