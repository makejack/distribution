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

        public PartnerApply(PartnerRole partnerRole, int totalQuantity)
        {
            PartnerRole = partnerRole;
            ApplyType = PartnerApplyType.Default;
            TotalQuantity = totalQuantity;
            Createat = DateTime.Now;
        }

        public PartnerRole PartnerRole { get; set; }
        public PartnerApplyType ApplyType { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime Createat { get; set; }

        public virtual List<PartnerApplyGoods> PartnerApplyGoods { get; set; }
    }
}