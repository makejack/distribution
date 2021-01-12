using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台订单列表响应
    /// </summary>
    public class AdminOrderListResponse
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
        /// 总优惠金额
        /// </summary>
        /// <value></value>
        public int TotalWithDiscount { get; set; }
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
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}