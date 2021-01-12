using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 装货申请请求
    /// </summary>
    public class ShipmentApplyRequest
    {
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
        /// <summary>
        /// 装货货品项
        /// </summary>
        /// <value></value>
        public List<ShipmentApplyOrderItemRequest> Items { get; set; }
    }
}