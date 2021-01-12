using System.Text;
using Microsoft.AspNetCore.Http;

namespace Mytime.Distribution.Extensions
{
    /// <summary>
    /// httprequest 扩展
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 获取域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetHostUrl(this HttpRequest request)
        {
            return new StringBuilder().Append(request.Scheme).Append("://").Append(request.Host).ToString();
        }

        /// <summary>
        /// 获取域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }
    }
}