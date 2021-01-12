using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台装货列表响应
    /// </summary>
    public class AdminShipmentListResponse : AdminShipmentResponse
    {
        /// <summary>
        /// 地址
        /// </summary>
        /// <value></value>
        public ShippingAddressResponse Address { get; set; }
    }
}