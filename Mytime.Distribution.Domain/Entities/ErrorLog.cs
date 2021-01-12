using System;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 错误日志
    /// </summary>
    public class ErrorLog : AggregateRoot
    {
        [Required]
        [MaxLength(512)]
        public string Url { get; set; }
        [Required]
        [MaxLength(32)]
        public string Method { get; set; }
        public string Body { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        [MaxLength(32)]
        public string IpAddress { get; set; }
        public DateTime Createat { get; set; }
    }
}