using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台商品列表响应
    /// </summary>
    public class AdminGoodsListResponse
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
        /// <summary>
        /// 价格
        /// </summary>
        /// <value></value>
        public int Price { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        /// <value></value>
        public int? ThumbnailImageId { get; set; }
        /// <summary>
        /// 主图地址
        /// </summary>
        /// <value></value>
        public string ThumbnailImageUrl { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        /// <value></value>
        public bool IsPublished { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        /// <value></value>
        public DateTime? PublishedOn { get; set; }
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
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}