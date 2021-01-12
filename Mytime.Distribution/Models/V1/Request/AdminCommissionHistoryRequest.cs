using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台佣金历史请求
    /// </summary>
    public class AdminCommissionHistoryRequest : PaginationRequest
    {
        /// <summary>
        /// 顾客Id
        /// </summary>
        /// <value></value>
        public int? CustomerId { get; set; }
        /// <summary>
        /// 佣金状态
        /// </summary>
        /// <value></value>
        public CommissionStatus? Status { get; set; }
    }
}