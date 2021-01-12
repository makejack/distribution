namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 合伙人申请商品创建请求
    /// </summary>
    public class AdminPartnerApplyGoodsCreateRequest
    {
        /// <summary>
        /// Id 没有时为0
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        /// <value></value>
        public int GoodsId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        /// <value></value>
        public int Price { get; set; }
    }
}