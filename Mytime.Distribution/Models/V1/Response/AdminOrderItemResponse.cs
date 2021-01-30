using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 订单商品项响应
    /// </summary>
    public class AdminOrderItemResponse
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
        /// 商品地址
        /// </summary>
        /// <value></value>
        public string GoodsMediaUrl { get; set; }
        /// <summary>
        /// 标准化名称
        /// </summary>
        /// <value></value>
        public string NormalizedName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <value></value>
        public OrderItemStatus Status { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <value></value>
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}