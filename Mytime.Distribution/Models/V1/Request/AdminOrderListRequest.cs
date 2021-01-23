using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台订单列表请求
    /// </summary>
    public class AdminOrderListRequest : PaginationRequest
    {
        /// <summary>
        /// 顾客Id
        /// </summary>
        /// <value></value>
        public int? CustomerId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <value></value>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        /// <value></value>
        public OrderStatus? Status { get; set; }
    }
}