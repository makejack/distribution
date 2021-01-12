using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台错误日志响应
    /// </summary>
    public class AdminLogErrorListResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        /// <value></value>
        public string Url { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        /// <value></value>
        public string Method { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        /// <value></value>
        public string IpAddress { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}