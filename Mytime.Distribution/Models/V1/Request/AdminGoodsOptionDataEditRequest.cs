namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台商品选项数据编辑请求
    /// </summary>
    public class AdminGoodsOptionDataEditRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
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