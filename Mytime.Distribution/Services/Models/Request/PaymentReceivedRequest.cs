using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Services.Models.Request
{
    /// <summary>
    /// 支付接收请求
    /// </summary>
    public class PaymentReceivedRequest
    {
        /// <summary>
        /// 订单号
        /// </summary>
        /// <value></value>
        public string OrderNo { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        /// <value></value>
        public int PaymentFee { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <value></value>
        public PaymentMethods PaymentMethod { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        /// <value></value>
        public DateTime PaymentTime { get; set; }
    }
}