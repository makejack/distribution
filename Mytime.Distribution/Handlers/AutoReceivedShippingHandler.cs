using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mytime.Distribution.Events;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Handlers
{
    /// <summary>
    /// 自动收货处理
    /// </summary>
    public class AutoReceivedShippingHandler : INotificationHandler<AutoReceivedShippingEvent>
    {
        private readonly IShipmentService _shipmentService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shipmentService"></param>
        public AutoReceivedShippingHandler(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(AutoReceivedShippingEvent notification, CancellationToken cancellationToken)
        {
            await _shipmentService.Confirm(notification.ShipmentId);
        }
    }
}