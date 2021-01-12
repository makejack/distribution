namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 商品列表查询请求
    /// </summary>
    public class AdminGoodsListQueryRequest : PaginationRequest
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        /// <value></value>
        public bool? IsPublished { get; set; }
    }
}