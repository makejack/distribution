using System.Collections.Generic;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 合伙人申请创建请求
    /// </summary>
    public class AdminPartnerApplyCreateRequest
    {
        /// <summary>
        /// 合伙人角色
        /// </summary>
        /// <value></value>
        public PartnerRole PartnerRole { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        /// <value></value>
        public int TotalQuantity { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        /// <value></value>
        public int OriginalPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        /// <value></value>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 推荐佣金比例
        /// </summary>
        /// <value></value>
        public int ReferralCommissionRatio { get; set; }
        /// <summary>
        /// 回购佣金比例
        /// </summary>
        /// <value></value>
        public int RepurchaseCommissionRatio { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        /// <value></value>

        public List<AdminPartnerApplyGoodsCreateRequest> Goods { get; set; }
    }
}