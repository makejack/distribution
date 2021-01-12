namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台产口创建选项值
    /// </summary>
    public class AdminGoodsCreateOptionValueParam
    {
        /// <summary>
        /// 值
        /// </summary>
        /// <value></value>
        public string Value { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        /// <value></value>
        public int DisplayOrder { get; set; }
    }
}