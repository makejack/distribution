using System;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 短信记录
    /// </summary>
    public class SmsLog : AggregateRoot
    {
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(32)]
        public string Tel { get; set; }
        [MaxLength(32)]
        public string Code { get; set; }
        [MaxLength(32)]
        public string MsgId { get; set; }
        [MaxLength(32)]
        public string Time { get; set; }
        [MaxLength(512)]
        public string Message { get; set; }
        [MaxLength(512)]
        public string ErrorMsg { get; set; }
        public DateTime Createat { get; set; }
    }
}