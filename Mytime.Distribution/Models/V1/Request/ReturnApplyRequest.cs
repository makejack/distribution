using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 退货申请请求
    /// </summary>
    public class ReturnApplyRequest
    {
        /// <summary>
        /// 发货商品Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 退货类型
        /// </summary>
        /// <value></value>
        public ReturnTypes ReturnType { get; set; }
        /// <summary>
        /// 物流状态
        /// </summary>
        /// <value></value>
        public ReturnLogisticsStatus LogisticsStatus { get; set; }
        /// <summary>
        /// 退货原因
        /// </summary>
        /// <value></value>
        public string Reason { get; set; }
    }
}