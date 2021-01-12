namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 商品选项查询请求
    /// </summary>
    public class AdminGoodsOptionQueryRequest : PaginationRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
    }
}