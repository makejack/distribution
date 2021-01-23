using System;
using System.Collections.Generic;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 合伙人申请条件响应
    /// </summary>
    public class PartnerApplyConditionResponse
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
        /// 原价
        /// </summary>
        /// <value></value>
        public int OriginalPrice { get; set; }
        /// <summary>
        /// 总价格
        /// </summary>
        /// <value></value>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        /// <value></value>
        public int TotalQuantity { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        /// <value></value>
        public List<AdminPartnerApplyGoodsGetResponse> Goods { get; set; }
    }
}