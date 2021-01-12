using System.Runtime.ConstrainedExecution;
namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取商品选项组合响应
    /// </summary>
    public class AdminGoodsGetOptionCombinationResponse
    {
        /// <summary>
        /// 选项Id
        /// </summary>
        /// <value></value>
        public int OptionId { get; set; }
        /// <summary>
        /// 选项名称
        /// </summary>
        /// <value></value>
        public string OptionName { get; set; }
        /// <summary>
        /// 选项值
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