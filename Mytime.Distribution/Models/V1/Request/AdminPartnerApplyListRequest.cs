using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 合伙人申请列表请求
    /// </summary>
    public class AdminPartnerApplyListRequest : PaginationRequest
    {
        /// <summary>
        /// 合伙人角色
        /// </summary>
        /// <value></value>
        public PartnerRole? PartnerRole { get; set; }
    }
}