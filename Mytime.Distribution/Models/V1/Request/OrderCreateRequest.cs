using System.Collections.Generic;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 订单创建请求
    /// </summary>
    public class OrderCreateRequest
    {
        /// <summary>
        /// 合伙人角色
        /// </summary>
        /// <value></value>
        public PartnerRole Role { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        /// <value></value>
        public PaymentTypes PaymentType { get; set; }
        
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
        
        /// <summary>
        /// 商品子项
        /// </summary>
        /// <value></value>
        public List<OrderCreateItemRequest> Items { get; set; }
    }
}