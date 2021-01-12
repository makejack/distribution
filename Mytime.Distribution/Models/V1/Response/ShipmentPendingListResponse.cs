using System.Collections.Generic;
using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 等待装货列表响应
    /// </summary>
    public class ShipmentPendingListResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <value></value>
        public int OrderId { get; set; }
        /// <summary>
        /// 商品号
        /// </summary>
        /// <value></value>
        public int? GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        /// <value></value>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        /// <value></value>
        public int GoodsPrice { get; set; }
        /// <summary>
        /// 优惠价格
        /// </summary>
        /// <value></value>
        public int DiscountAmount { get; set; }
        /// <summary>
        /// 商品主图
        /// </summary>
        /// <value></value>
        public string GoodsMediaUrl { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        /// <value></value>
        public List<AdminGoodsGetOptionResponse> Options { get; set; }
        /// <summary>
        /// 组合
        /// </summary>
        /// <value></value>
        public List<AdminGoodsGetVariationResponse> Variations { get; set; }
    }
}