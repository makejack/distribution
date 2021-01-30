using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 退款详情响应
    /// </summary>
    public class RefundGetResponse
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
        /// 商品主图
        /// </summary>
        /// <value></value>
        public string GoodsMediaUrl { get; set; }
        /// <summary>
        /// 标准化名称
        /// </summary>
        /// <value></value>
        public string NormalizedName { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <value></value>
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 退款状态 1=申请失败 ,2=退货申请,3=确认申请,4=退还商品,5=完成退货
        /// </summary>
        /// <value></value>
        public OrderItemStatus RefundStatus { get; set; }
        /// <summary>
        /// 退货理由
        /// </summary>
        /// <value></value>
        public string RefundReason { get; set; }
        /// <summary>
        /// 退货申请时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundApplyTime { get; set; }
        /// <summary>
        /// 退货时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        /// <value></value>
        public int RefundAmount { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        /// <value></value>
        public ReturnAddressResponse Address { get; set; }
    }
}