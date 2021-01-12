using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mytime.Distribution.Events;
using Mytime.Distribution.Services;
using Mytime.Distribution.Services.Models.Request;

namespace Mytime.Distribution.Handlers
{
    /// <summary>
    /// 支付接收处理
    /// </summary>
    public class PaymentReceivedHandler : INotificationHandler<PaymentReceivedEvent>
    {
        private readonly IOrderService _orderService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderService"></param>
        public PaymentReceivedHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(PaymentReceivedEvent notification, CancellationToken cancellationToken)
        {
            await _orderService.PaymentReceived(new PaymentReceivedRequest
            {
                OrderNo = notification.OrderNo,
                PaymentFee = notification.PaymentFee,
                PaymentMethod = notification.PaymentMethod,
                PaymentTime = notification.PaymentTime
            });
        }
    }
}