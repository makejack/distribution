using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取装货详情响应
    /// </summary>
    public class AdminShipmentGetResponse : AdminShipmentResponse
    {
        /// <summary>
        /// 发货的商品
        /// </summary>
        /// <value></value>
        public List<ShipmentListOrderItemResponse> Items { get; set; }
        /// <summary>
        /// 送货地址
        /// </summary>
        /// <value></value>
        public ShippingAddressResponse Address { get; set; }
    }
}