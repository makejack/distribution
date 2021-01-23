namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 提现银行卡请求
    /// </summary>
    public class WithdrawalPayBankRequest
    {
        /// <summary>
        /// 银行卡Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        /// <value></value>
        public int Amount { get; set; }
    }
}