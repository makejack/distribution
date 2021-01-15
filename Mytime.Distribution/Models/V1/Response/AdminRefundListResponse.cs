using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台退款列表响应
    /// </summary>
    public class AdminRefundListResponse
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
        /// 退款状态
        /// </summary>
        /// <value></value>
        public RefundStatus RefundStatus { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        /// <value></value>
        public int RefundAmount { get; set; }
        /// <summary>
        /// 退款申请时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundApplyTime { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <value></value>
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        /// <value></value>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <value></value>
        public string OrderNo { get; set; }
        /// <summary>
        /// 买家Id
        /// </summary>
        /// <value></value>
        public int CustomerId { get; set; }
        /// <summary>
        /// 买家名称
        /// </summary>
        /// <value></value>
        public string CustomerName { get; set; }
        /// <summary>
        /// 买家头像
        /// </summary>
        /// <value></value>
        public string AvatarUrl { get; set; }
    }
}