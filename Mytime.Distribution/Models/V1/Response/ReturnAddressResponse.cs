namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 退货地址响应
    /// </summary>
    public class ReturnAddressResponse : ShippingAddressResponse
    {
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
    }
}