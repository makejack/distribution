using System.Collections.Generic;
using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Domain.Entities
{
    public class PartnerApply : AggregateRoot
    {
        public PartnerApply()
        {
        }


        public PartnerRole PartnerRole { get; set; }
        public PartnerApplyType ApplyType { get; set; }
        /// <summary>
        /// 推荐佣金比例
        /// </summary>
        /// <value></value>
        public int ReferralCommissionRatio { get; set; }
        /// <summary>
        /// 回购佣金比例
        /// </summary>
        /// <value></value>
        public int RepurchaseCommissionRatio { get; set; }
        public int TotalQuantity { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        /// <value></value>
        public int OriginalPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        /// <value></value>
        public int TotalAmount { get; set; }
        public DateTime Createat { get; set; }

        public virtual List<PartnerApplyGoods> PartnerApplyGoods { get; set; }
    }
}