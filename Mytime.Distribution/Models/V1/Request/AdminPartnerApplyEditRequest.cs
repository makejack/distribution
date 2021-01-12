using System.Collections.Generic;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 合伙人申请编辑请求
    /// </summary>
    public class AdminPartnerApplyEditRequest
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
        /// 商品
        /// </summary>
        /// <value></value>
        public List<AdminPartnerApplyGoodsCreateRequest> Goods { get; set; }
    }
}