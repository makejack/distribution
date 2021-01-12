using System.Threading.Tasks;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 快递服务
    /// </summary>
    public interface IPackageService
    {
        /// <summary>
        /// 快递查询
        /// </summary>
        /// <param name="com">快递公司</param>
        /// <param name="num">快递单号</param>
        /// <param name="phone">收、寄件人手机号</param>
        /// <returns></returns>
        Task<string> QueryAsync(string com, string num, string phone);
    }
}