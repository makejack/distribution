namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台分类创建请求
    /// </summary>
    public class AdminCategoryCreateRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <value></value>
        public int Sort { get; set; }
    }
}