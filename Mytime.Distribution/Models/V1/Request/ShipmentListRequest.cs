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
        /// </summary>
        /// <value></value>
        public ShippingStatus Status { get; set; }
    }
}