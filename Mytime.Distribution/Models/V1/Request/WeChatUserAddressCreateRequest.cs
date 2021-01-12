namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 微信用户地址
    /// </summary>
    public class WeChatUserAddressCreateRequest
    {
        /// <summary>
        /// 是否默认
        /// </summary>
        /// <value></value>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        /// <value></value>
        public int PostalCode { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        /// <value></value>
        public int ProvinceCode { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        /// <value></value>
        public int CityCode { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        /// <value></value>
        public int AreaCode { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        /// <value></value>
        public string DetailInfo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public string TelNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }
    }
}