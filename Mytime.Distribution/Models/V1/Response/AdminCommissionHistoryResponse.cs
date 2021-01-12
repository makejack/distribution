namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台佣金历史记录
    /// </summary>
    public class AdminCommissionHistoryResponse : CommissionHistoryResponse
    {
        /// <summary>
        /// 人员
        /// </summary>
        /// <value></value>
        public CustomerResponse Customer { get; set; }
    }
}