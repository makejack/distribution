namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台请求响应日志信息数据
    /// </summary>
    public class AdminLogGetResponse : AdminLogListResponse
    {
        /// <summary>
        /// 头部内容
        /// </summary>
        /// <value></value>
        public string Headers { get; set; }
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
    }
}