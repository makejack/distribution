namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 订单创建响应
    /// </summary>
    public class OrderCreateResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <value></value>
        public string OrderNo { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        /// <value></value>
        public int TotalFee { get; set; }
        /// <summary>
        /// 实付价格
        /// </summary>
        /// <value></value>
        public int ActuallyAmount { get; set; }
    }
}