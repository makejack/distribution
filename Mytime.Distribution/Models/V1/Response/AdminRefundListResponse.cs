using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台退款列表响应
    /// </summary>
    public class AdminRefundListResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        /// <value></value>
        public ReturnAuditStatus Status { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <value></value>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        /// <value></value>
        public int RefundAmount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
        /// <summary>
        /// 买家Id
        /// </summary>
        /// <value></value>
        public int CustomerId { get; set; }
        /// <summary>
        /// 买家名称
        /// </summary>
        /// <value></value>
        public string CustomerName { get; set; }
        /// <summary>
        /// 买家头像
        /// </summary>
        /// <value></value>
        public string AvatarUrl { get; set; }
    }
}