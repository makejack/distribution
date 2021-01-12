namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 装货申请订单子项请求
    /// </summary>
    public class ShipmentApplyOrderItemRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        /// <value></value>
        public int GoodsId { get; set; }
        /// <summary>
        /// 标准化名称
        /// </summary>
        /// <value></value>
        public string NormalizedName { get; set; }
    }
}