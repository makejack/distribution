using System.Collections.Generic;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台创建订单请求
    /// </summary>
    public class AdminOrderCreateRequest
    {
        /// <summary>
        /// 微信用户Id
        /// </summary>
        /// <value></value>
        public int CustomerId { get; set; }

        /// <summary>
        /// 合伙人角色
        /// </summary>
        /// <value></value>
        public PartnerRole PartnerRole { get; set; }

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
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        /// <value></value>
        public List<OrderCreateItemRequest> Goods { get; set; }
    }
}