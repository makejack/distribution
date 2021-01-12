namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 送货查询响应
    /// </summary>
    public class ShippingQueryResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 快买公司
        /// </summary>
        /// <value></value>
        public string CourierCompany { get; set; }
        /// <summary>
        /// 快递公司Code
        /// </summary>
        /// <value></value>
        public string CourierCompanyCode { get; set; }
        /// <summary>
        /// 快递号
        /// </summary>
        /// <value></value>
        public string TrackingNumber { get; set; }
        /// <summary>
        /// 物流信息
        /// </summary>
        /// <value></value>
        public string Body { get; set; }
    }
}