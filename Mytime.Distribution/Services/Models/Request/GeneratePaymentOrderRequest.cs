namespace Mytime.Distribution.Services.Models.Request
{
    /// <summary>
    /// 生成支付订单请求
    /// </summary>
    public class GeneratePaymentOrderRequest
    {
        /// <summary>
        /// 订单号
        /// </summary>
        /// <value></value>
        public string OrderNo { get; set; }
        /// <summary>
        ///标题
        /// </summary>
        /// <value></value>
        public string Subject { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        /// <value></value>
        public int TotalFee { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        /// <value></value>
        public string OpenId { get; set; }
    }
}