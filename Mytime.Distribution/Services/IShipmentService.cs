using System.Threading.Tasks;
using Mytime.Distribution.Models;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 装货服务
    /// </summary>
    public interface IShipmentService
    {
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        Task<Result> Confirm(int shipmentId);
    }
}