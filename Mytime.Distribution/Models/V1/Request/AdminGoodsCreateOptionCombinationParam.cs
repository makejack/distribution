namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台商品创建选项组合参数
    /// </summary>
    public class AdminGoodsCreateOptionCombinationParam
    {
        /// <summary>
        /// 选项Id
        /// </summary>
        /// <value></value>
        public int OptionId { get; set; }
        /// <summary>
        /// 选项数据值
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