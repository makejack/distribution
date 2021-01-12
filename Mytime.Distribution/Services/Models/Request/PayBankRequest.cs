namespace Mytime.Distribution.Services.Models.Request
{
    /// <summary>
    /// 提现到银行卡请求
    /// </summary>
    public class PayBankRequest
    {
        /// <summary>
        /// 商户企业付款单号
        /// </summary>
        /// <value></value>
        public string PartnerTradeNo { get; set; }
        /// <summary>
        /// 收款方银行卡号
        /// </summary>
        /// <value></value>
        public string BankNo { get; set; }
        /// <summary>
        /// 收款方用户名
        /// </summary>
        /// <value></value>
        public string TrueName { get; set; }
        /// <summary>
        /// 收款方开户行
        /// </summary>
        /// <value></value>
        public string BankCode { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        /// <value></value>
        public int Amount { get; set; }
        /// <summary>
        /// 付款备注
        /// </summary>
        /// <value></value>
        public string Desc { get; set; }
    }
}