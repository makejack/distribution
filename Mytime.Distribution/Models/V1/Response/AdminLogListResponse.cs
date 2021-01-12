using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台请求响应列表数据
    /// </summary>
    public class AdminLogListResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        /// <value></value>
        public string Url { get; set; }
        /// <summary>
        /// 方式
        /// </summary>
        /// <value></value>
        public string Method { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}