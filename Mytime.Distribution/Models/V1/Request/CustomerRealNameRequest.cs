namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 用户实名请求
    /// </summary>
    public class CustomerRealNameRequest
    {
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
        /// 信息银行编号
        /// </summary>
        /// <value></value>
        public string BankCode { get; set; }
    }
}