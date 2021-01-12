using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Configs;
using Mytime.Distribution.Events;
using Mytime.Distribution.Utils.Constants;
using Newtonsoft.Json;

namespace Mytime.Distribution.HostedServices
{
    /// <summary>
    /// 支付接收侦听
    /// </summary>
    public class PaymentReceivedListener : RabbitMQListener
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public PaymentReceivedListener(
            IServiceProvider serviceProvider,
            IOptions<RabbitMQConnectionConfig> options,
            ILogger<RabbitMQListener> logger) : base(options, logger)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// RabbitMQ消息处理
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async override Task<bool> Process(string routingKey, string message)
        {
            INotification notification = null;

            switch (routingKey)
            {
                case MQConstants.PaymentTimeOutRouteKey:
                    notification = JsonConvert.DeserializeObject<PaymentTimeoutEvent>(message);
                    break;
                case MQConstants.AutoReceivedShippingKey:
                    notification = JsonConvert.DeserializeObject<AutoReceivedShippingEvent>(message);
                    break;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                await mediator.Publish(notification);
            }

            return true;
        }
    }
}