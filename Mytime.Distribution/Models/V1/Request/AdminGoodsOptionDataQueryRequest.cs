namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 商品选项数据查询请求
    /// </summary>
    public class AdminGoodsOptionDataQueryRequest : PaginationRequest
    {
        /// <summary>
        /// 选项Id
        /// </summary>
        /// <value></value>
        public int OptionId { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        /// <value></value>
        public string Value { get; set; }
    }
}