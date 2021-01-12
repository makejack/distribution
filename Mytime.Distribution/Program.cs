using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Mytime.Distribution
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("nlog.config");

            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// CreateHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureLogging(logging =>
                // {
                //     logging.ClearProviders(); //移除已经注册的其他日志处理程序
                //     logging.SetMinimumLevel(LogLevel.Trace); //设置最小的日志级别
                // })

                .ConfigureWebHostDefaults(webBuilder =>
                {
#if !DEBUG
                    webBuilder.UseUrls($"http://localhost:5020;https://localhost:5021");
#endif
                    webBuilder.UseStartup<Startup>(); //.UseDefaultServiceProvider(options => options.ValidateScopes = false)
                }).UseNLog();
    }
}
