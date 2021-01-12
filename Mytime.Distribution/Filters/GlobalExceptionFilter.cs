using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;

namespace Mytime.Distribution.Filters
{
    /// <summary>
    /// 统一异常过滤
    /// </summary>
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IRepositoryByInt<ErrorLog> _errorLogRepository;
        private readonly ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorLogRepository"></param>
        /// <param name="logger"></param>
        public GlobalExceptionFilter(IRepositoryByInt<ErrorLog> errorLogRepository,
                                     ILogger<GlobalExceptionFilter> logger)
        {
            _errorLogRepository = errorLogRepository;
            _logger = logger;
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="context"></param>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            try
            {
                var exception = context.Exception;
                var request = context.HttpContext.Request;
                string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                string body = string.Empty;
                string queryString = request.QueryString.Value;
                if (request.Method.Equals("post", StringComparison.InvariantCultureIgnoreCase)
                || request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (request.ContentType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (var mem = new MemoryStream())
                        {
                            using (var reader = new StreamReader(mem))
                            {
                                request.Body.Seek(0, SeekOrigin.Begin);
                                request.Body.CopyTo(mem);
                                mem.Seek(0, SeekOrigin.Begin);
                                body = reader.ReadToEnd();
                            }
                        }
                    }
                }
                var log = new ErrorLog()
                {
                    Url = request.Path + request.QueryString,
                    Body = body,
                    Method = request.Method,
                    Message = exception.Message,
                    Detail = exception.StackTrace,
                    IpAddress = ip,
                    Createat = DateTime.Now
                };
                await _errorLogRepository.InsertAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }
            finally
            {
                //异常已处理
                context.ExceptionHandled = true;

                var result = new Result(ResultCodes.SysError);

#if DEBUG

                result.ErrorMsg = context.Exception.Message;

#endif

                // 返回异常的内容
                context.Result = new ObjectResult(result);
            }
        }
    }
}