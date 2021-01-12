namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台首页数据统计响应
    /// </summary>
    public class AdminHomeDataStatisticsResponse
    {
        /// <summary>
        /// 总销售额
        /// </summary>
        /// <value></value>
        public int TotalSales { get; set; }
        /// <summary>
        /// 总订单数
        /// </summary>
        /// <value></value>
        public int TotalOrders { get; set; }
        /// <summary>
        /// 待发货
        /// </summary>
        /// <value></value>
        public int PendingShipmentCount { get; set; }
        /// <summary>
        /// 总合伙人数
        /// </summary>
        /// <value></value>
        public int TotalCustomer { get; set; }
    }
}