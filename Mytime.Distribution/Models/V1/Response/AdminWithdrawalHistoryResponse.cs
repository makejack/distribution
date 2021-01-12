namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台佣金历史记录响应
    /// </summary>
    public class AdminWithdrawalHistoryResponse : WithdrawalHistoryResponse
    {
        /// <summary>
        /// 用户
        /// </summary>
        /// <value></value>
        public CustomerResponse Customer { get; set; }
    }
}