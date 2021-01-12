using Newtonsoft.Json;

namespace Mytime.Distribution.Services.Models.Response
{
    /// <summary>
    /// 支付订单响应
    /// </summary>
    public class PaymentOrderResponse
    {
        /// <summary>
        /// timeStamp
        /// </summary>
        /// <value></value>
        [JsonProperty("timeStamp")]
        public string TimeStamp { get; set; }
        /// <summary>
        /// nonceStr
        /// </summary>
        /// <value></value>
        [JsonProperty("nonceStr")]
        public string NonceStr { get; set; }
        /// <summary>
        /// package
        /// </summary>
        /// <value></value>
        [JsonProperty("package")]
        public string Package { get; set; }
        /// <summary>
        /// signType
        /// </summary>
        /// <value></value>
        [JsonProperty("signType")]
        public string SignType { get; set; }
        /// <summary>
        /// paySign
        /// </summary>
        /// <value></value>
        [JsonProperty("paySign")]
        public string PaySign { get; set; }
    }
}