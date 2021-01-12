using System.Collections.Generic;
using System;
using Mytime.Distribution.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    public class Customer : AggregateRoot
    {
        public Customer()
        {
        }

        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string NickName { get; set; }
        [MaxLength(128)]
        public string OpenId { get; set; }
        [MaxLength(32)]
        public string SessionKey { get; set; }
        [MaxLength(512)]
        public string UnionId { get; set; }
        [MaxLength(32)]
        public string PhoneNumber { get; set; }
        [MaxLength(32)]
        public string CountryCode { get; set; }
        public int Gender { get; set; }
        [MaxLength(512)]
        public string Country { get; set; }
        [MaxLength(512)]
        public string Province { get; set; }
        [MaxLength(512)]
        public string City { get; set; }
        [MaxLength(512)]
        public string AvatarUrl { get; set; }
        [MaxLength(32)]
        public string Language { get; set; }
        public PartnerRole Role { get; set; }
        public byte Status { get; set; }
        public int? ParentId { get; set; }
        /// <summary>
        /// 银行卡编号
        /// </summary>
        /// <value></value>
        [MaxLength(32)]
        public string BankNo { get; set; }
        /// <summary>
        /// 微信银行编号
        /// </summary>
        /// <value></value>
        [MaxLength(32)]
        public string BankCode { get; set; }
        /// <summary>
        /// 是否实名
        /// </summary>
        /// <value></value>
        public bool IsRealName { get; set; }
        public DateTime Createat { get; set; }

        public virtual Assets Assets { get; set; }
        public virtual List<WithdrawalHistory> WithdrawalHistorys { get; set; }
        public virtual List<CustomerRelation> CustomerRelationChildrens { get; set; }
        public virtual List<Orders> Orders { get; set; }

    }
}