namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台历史记录请求
    /// </summary>
    public class AdminWithdrawalHistoryRequest : PaginationRequest
    {
        /// <summary>
        /// 顾客Id
        /// </summary>
        /// <value></value>
        public int? CustomerId { get; set; }
    }
}