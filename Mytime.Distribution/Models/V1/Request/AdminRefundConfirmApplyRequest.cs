using System;
namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台确认退款申请
    /// </summary>
    public class AdminRefundConfirmApplyRequest
    {
        /// <summary>
        /// 退款商品Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 退款地址
        /// </summary>
        /// <value></value>
        public int RefundAddressId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remarks { get; set; }
    }
}