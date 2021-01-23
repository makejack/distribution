using Mytime.Distribution.Domain.Shared;
using System;
using System.Collections.Generic;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取合伙人申请响应
    /// </summary>
    public class AdminPartnerApplyGetResponse : AdminPartnerApplyListResponse
    {
        /// <summary>
        /// 商品
        /// </summary>
        /// <value></value>
        public List<AdminPartnerApplyGoodsGetResponse> Goods { get; set; }
    }
}