using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台合伙人申请列表响应
    /// </summary>
    public class AdminPartnerApplyListResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 合伙人角色
        /// </summary>
        /// <value></value>
        public PartnerRole PartnerRole { get; set; }
        /// <summary>
        /// 申请类型
        /// </summary>
        /// <value></value>
        public PartnerApplyType ApplyType { get; set; }
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
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}