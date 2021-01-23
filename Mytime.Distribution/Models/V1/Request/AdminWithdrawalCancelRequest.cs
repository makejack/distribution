using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台提现确认请求
    /// </summary>
    public class AdminWithdrawalCancelRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
    }
}