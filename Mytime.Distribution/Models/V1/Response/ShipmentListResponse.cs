using System.Collections.Generic;
using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 装货列表响应
    /// </summary>
    public class ShipmentListResponse : ShipmentResponse
    {
        /// <summary>
        /// 装货商品
        /// </summary>
        /// <value></value>
        public List<ShipmentListOrderItemResponse> Items { get; set; }
    }
}