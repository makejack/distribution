using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台商品创建请求
    /// </summary>
    public class AdminGoodsCreateRequest
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        /// <value></value>
        public int Price { get; set; }
        /// <summary>
        /// 主图Id
        /// </summary>
        /// <value></value>
        public int? ThumbnailImageId { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        /// <value></value>
        public bool IsPublished { get; set; }
        /// <summary>
        /// 城市合伙人折扣
        /// </summary>
        /// <value></value>
        public int CityDiscount { get; set; }
        /// <summary>
        /// 网点合伙人折扣
        /// </summary>
        public int BranchDiscount { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        /// <value></value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        /// <value></value>
        public List<int> GoodsMediaIds { get; set; }
        /// <summary>
        /// 商品选项
        /// </summary>
        /// <value></value>
        public List<AdminGoodsCreateOptionParam> Options { get; set; }
        /// <summary>
        /// 商品组合
        /// </summary>
        /// <value></value>
        public List<AdminGoodsCreateVariationParam> Variations { get; set; }
    }
}