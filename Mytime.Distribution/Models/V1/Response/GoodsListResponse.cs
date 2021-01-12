using System;
using System.Collections.Generic;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 商品列表响应
    /// </summary>
    public class GoodsListResponse
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
        /// 优惠价格
        /// </summary>
        /// <value></value>
        public int DiscountPrice { get; set; }
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
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        /// <value></value>
        public List<MediaResponse> GoodsMedias { get; set; }
    }
}