using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Mytime.Distribution.Extensions
{
    /// <summary>
    /// http 扩展
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static int GetUserId(this HttpContext httpContext)
        {
            var strId = httpContext.User.Claims.FirstOrDefault(e => e.Type == "id").Value;
            return int.Parse(strId);
        }
    }
}