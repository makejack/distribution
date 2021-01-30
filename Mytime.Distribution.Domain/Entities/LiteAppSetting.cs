using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 小程序设置
    /// </summary>
    public class LiteAppSetting : AggregateRoot
    {

        /// <summary>
        /// 城市权益
        /// </summary>
        /// <value></value>
        public string CityMembershipRights { get; set; }

        /// <summary>
        /// 网点权益
        /// </summary>
        /// <value></value>
        public string BranchMembershipRights { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}