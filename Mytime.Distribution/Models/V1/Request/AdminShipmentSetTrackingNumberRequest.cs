namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台装货设置快递单号
    /// </summary>
    public class AdminShipmentSetTrackingNumberRequest
    {
        /// <summary>
        /// ShipmentId
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
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
    }
    
}