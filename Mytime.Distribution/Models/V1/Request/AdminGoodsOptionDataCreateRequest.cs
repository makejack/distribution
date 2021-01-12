namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台商品选项数据创建请求
    /// </summary>
    public class AdminGoodsOptionDataCreateRequest
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
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
    }
}