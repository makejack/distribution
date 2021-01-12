using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台商品创建组合
    /// </summary>
    public class AdminGoodsCreateVariationParam
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 标准化名称 如：红 8G
        /// </summary>
        /// <value></value>
        public string NormalizedName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        /// <value></value>
        public int Price { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        /// <value></value>
        public int StockQuantity { get; set; }
        /// <summary>
        /// 选项组合
        /// </summary>
        /// <value></value>
        public List<AdminGoodsCreateOptionCombinationParam> OptionCombinations { get; set; }
    }
}