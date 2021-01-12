using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 提现历史记录响应
    /// </summary>
    public class WithdrawalHistoryResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        /// <value></value>
        public int Total { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        /// <value></value>
        public int Amount { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        /// <value></value>
        public int HandlingFee { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <value></value>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}