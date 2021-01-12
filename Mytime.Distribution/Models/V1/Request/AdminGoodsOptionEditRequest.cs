namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 商品选项编辑请求
    /// </summary>
    public class AdminGoodsOptionEditRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
    }
}