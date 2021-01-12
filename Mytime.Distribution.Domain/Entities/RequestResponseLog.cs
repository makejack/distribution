using System;
using System.ComponentModel.DataAnnotations;

namespace Mytime.Distribution.Domain.Entities
{
    /// <summary>
    /// 请求响应记录
    /// </summary>
    public class RequestResponseLog : AggregateRoot
    {
        /// <summary>
        /// 地址
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(512)]
        public string Url { get; set; }
        /// <summary>
        /// 头部内容
        /// </summary>
        /// <value></value>
        public string Headers { get; set; }
        /// <summary>
        /// 方式
        /// </summary>
        /// <value></value>
        [MaxLength(32)]
        public string Method { get; set; }
        /// <summary>
        /// 请求数据
        /// </summary>
        /// <value></value>
        public string RequestBody { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        /// <value></value>
        public string ResponseBody { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}