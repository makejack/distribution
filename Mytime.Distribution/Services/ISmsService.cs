using System.Threading.Tasks;
using Mytime.Distribution.Services.SmsContent;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 短信服务
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        Task SendAsync(string tel, INotify notify);
    }
}