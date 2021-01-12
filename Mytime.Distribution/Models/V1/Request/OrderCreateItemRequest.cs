namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 订单创建子项请求
    /// </summary>
    public class OrderCreateItemRequest
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
    }
}