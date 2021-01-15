using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 装货列表请求
    /// </summary>
    public class ShipmentListRequest : PaginationRequest
    {
        /// <summary>
        /// 状态
        /// 100 等待发货， 101 已出货 , 200 完成
        /// </summary>
        /// <value></value>
        public ShippingStatus Status { get; set; }
    }
}