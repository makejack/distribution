namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 装货地址
    /// </summary>
    public class ShipmentAddressRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 送货地址Id
        /// </summary>
        /// <value></value>
        public int ShippingAddressId { get; set; }
        /// <summary>
        /// 微信送货地址
        /// </summary>
        /// <value></value>
        public ShippingAddressRequest ShippingAddress { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
    }
}