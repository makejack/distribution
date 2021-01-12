using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取商品组合响应
    /// </summary>
    public class AdminGoodsGetVariationResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 标准化名称
        /// </summary>
        /// <value></value>
        public string NormalizedName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        /// <value></value>
        public int Price { get; set; }
        /// <summary>
        /// 优惠价格
        /// </summary>
        /// <value></value>
        public int DiscountPrice { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        /// <value></value>
        public int StockQuantity { get; set; }
        /// <summary>
        /// 选项组合
        /// </summary>
        /// <value></value>
        public List<AdminGoodsGetOptionCombinationResponse> OptionCombinations { get; set; }
    }
}