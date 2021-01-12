using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 媒体响应
    /// </summary>
    public class MediaResponse
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
        /// 类型
        /// </summary>
        /// <value></value>
        public string Type { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        /// <value></value>
        public long Size { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}