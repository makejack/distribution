using System;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 用户在资产
    /// </summary>
    public class Assets : AggregateRoot
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 总资产
        /// </summary>
        /// <value></value>
        public int TotalAssets { get; set; }
        /// <summary>
        /// 可用金额
        /// </summary>
        /// <value></value>
        public int AvailableAmount { get; set; }
        /// <summary>
        /// 总佣金
        /// </summary>
        /// <value></value>
        public int TotalCommission { get; set; }
        /// <summary>
        /// 回购金额
        /// 提现的20%用于回购商品
        /// </summary>
        /// <value></value>
        public int RepurchaseAmount { get; set; }

        public DateTime UpdateTime { get; set; }
        public DateTime Createat { get; set; }

    }
}