using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台合伙人申请商品响应
    /// </summary>
    public class AdminPartnerApplyGoodsGetResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        /// <value></value>
        public int GoodsId { get; set; }
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
        /// 原价
        /// </summary>
        /// <value></value>
        public int OriginalPrice { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        /// <value></value>
        public string ThumbnailImageUrl { get; set; }
        /// <summary>
        /// 数量 
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}