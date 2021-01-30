namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台拒绝退货请求
    /// </summary>
    public class AdminRefundRefuseApplyRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 审核消息
        /// </summary>
        /// <value></value>
        public string AuditMessage { get; set; }
    }
}