namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台分类编辑请求
    /// </summary>
    public class AdminCategoryEditRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 情况提前
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