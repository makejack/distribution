namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 提现获取银行卡响应
    /// </summary>
    public class BankCardResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 银行卡编号
        /// </summary>
        /// <value></value>
        public string BankNo { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        /// <value></value>
        public string BankName { get; set; }
        /// <summary>
        /// 微信银行编号
        /// </summary>
        /// <value></value>
        public string BankCode { get; set; }
    }
}