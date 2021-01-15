using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台修改订单信息
    /// </summary>
    public class AdminOrderEditRequest
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <value></value>
        public PaymentMethods PaymentMethod { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        /// <value></value>
        public PaymentTypes PaymentType { get; set; }

    }
}