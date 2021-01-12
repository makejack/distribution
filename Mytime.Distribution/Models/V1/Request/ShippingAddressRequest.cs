namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 微信送货地址
    /// </summary>
    public class ShippingAddressRequest
    {
        /// <summary>
        /// 邮政编码
        /// </summary>
        /// <value></value>
        public string PostalCode { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        /// <value></value>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        /// <value></value>
        public string CityName { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        /// <value></value>
        public string CountyName { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        /// <value></value>
        public string DetailInfo { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <value></value>
        public string TelNumber { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }
    }
}