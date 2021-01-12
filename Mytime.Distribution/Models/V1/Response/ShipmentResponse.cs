using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 装货基本参数响应
    /// </summary>
    public class ShipmentResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <value></value>
        public ShippingStatus ShippingStatus { get; set; }
        /// <summary>
        /// 装货时间
        /// </summary>
        /// <value></value>
        public DateTime? ShippingTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}