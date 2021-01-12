using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台装数据货响应
    /// </summary>
    public class AdminShipmentResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 总重量
        /// </summary>
        /// <value></value>
        public int TotalWeight { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        /// <value></value>
        public string CourierCompany { get; set; }
        /// <summary>
        /// 快递公司Code
        /// </summary>
        /// <value></value>
        public string CourierCompanyCode { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        /// <value></value>
        public string TrackingNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
        /// <summary>
        /// 发货状态
        /// </summary>
        /// <value></value>
        public ShippingStatus ShippingStatus { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        /// <value></value>
        public DateTime? ShippingTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <value></value>
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}