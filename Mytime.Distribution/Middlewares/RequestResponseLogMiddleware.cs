using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mytime.Distribution.Middlewares
{
    /// <summary>
    /// 请求响应记录中间件
    /// /// </summary>
    public class RequestResponseLogMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly string[] contextTypes = new[] { "application/json", "text/xml" };

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        public RequestResponseLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="repository"></param>
        public async Task InvokeAsync(HttpContext context, IRepositoryByInt<RequestResponseLog> repository)
        {
            context.Request.EnableBuffering();

            await LogRequest(context, repository);

            await _next(context);
        }

        private async Task LogRequest(HttpContext context, IRepositoryByInt<RequestResponseLog> repository)
        {
            var request = context.Request;

            // var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
            // var controllerName = controllerActionDescriptor.ControllerName;
            // var actionName = controllerActionDescriptor.ActionName;

            var url = request.Path.ToString();
            if (url.Contains("upload/images")) return;
            if (request.RouteValues.Count == 0) return;

            var log = new RequestResponseLog();
            log.Url = url;
            log.Method = request.Method;
            log.Headers = JsonConvert.SerializeObject(request.Headers);
            log.Createat = DateTime.Now;

            if (request.Method.Equals("get", StringComparison.InvariantCultureIgnoreCase))
            {
                log.RequestBody = request.QueryString.Value;
            }
            else if (request.Method.Equals("post", StringComparison.InvariantCultureIgnoreCase)
             || request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase))
            {
                var stream = request.Body;
                stream.Seek(0, SeekOrigin.Begin);

                var bytes = new byte[request.ContentLength.Value];
                stream.Read(bytes, 0, bytes.Length);
                log.RequestBody = Encoding.UTF8.GetString(bytes);

                stream.Seek(0, SeekOrigin.Begin);
            }

            await repository.InsertAsync(log);
        }
    }
}