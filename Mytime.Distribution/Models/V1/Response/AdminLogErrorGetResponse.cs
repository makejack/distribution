namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取错误日志说明响应
    /// </summary>
    public class AdminLogErrorGetResponse : AdminLogErrorListResponse
    {
        /// <summary>
        /// 请求内容
        /// </summary>
        /// <value></value>
        public string Body { get; set; }
        /// <summary>
        /// 错误内容
        /// </summary>
        /// <value></value>
        public string Detail { get; set; }
    }
}