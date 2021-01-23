namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 用户实名请求
    /// </summary>
    public class CustomerRealNameRequest
    {
        /// <summary>
        /// 微信实名真实姓名
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public string PhoneNumber { get; set; }
    }
}