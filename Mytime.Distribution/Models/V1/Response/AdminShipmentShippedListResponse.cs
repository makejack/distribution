namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台已发货列表响应
    /// </summary>
    public class AdminShipmentShippedListResponse : ShipmentListResponse
    {
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
    }
}