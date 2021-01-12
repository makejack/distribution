using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Events;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Handlers
{
    /// <summary>
    /// 支付超时处理
    /// </summary>
    public class PaymentTimeoutHandler : INotificationHandler<PaymentTimeoutEvent>
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderService"></param>
        public PaymentTimeoutHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(PaymentTimeoutEvent notification, CancellationToken cancellationToken)
        {
            await _orderService.Cancel(notification.OrderId, "超时取消支付");
        }
    }
}