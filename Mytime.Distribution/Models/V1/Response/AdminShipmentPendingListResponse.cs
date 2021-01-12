namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台待装货列表响应
    /// </summary>
    public class AdminShipmentPendingListResponse : ShipmentListResponse
    {
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
    }
}