namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台分类查询请求
    /// </summary>
    public class AdminCategoryQueryRequest : PaginationRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
    }
}