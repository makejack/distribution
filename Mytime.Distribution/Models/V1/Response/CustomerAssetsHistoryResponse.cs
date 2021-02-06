using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 用户资产历史记录
    /// </summary>
    public class CustomerAssetsHistoryResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 总金额（余额）
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 消费
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createat { get; set; }
    }
}