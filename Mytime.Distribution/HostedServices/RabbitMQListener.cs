using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Configs;
using Mytime.Distribution.Utils.Constants;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Mytime.Distribution.HostedServices
{
    /// <summary>
    /// RabbitMQ侦听
    /// </summary>
    public class RabbitMQListener : IHostedService
    {
        /// <summary>
        /// Represents a type used to perform logging.
        /// </summary>
        protected ILogger _logger;
        /// <summary>
        /// Main interface to an AMQP connection.
        /// </summary>
        protected IConnection _connection;
        /// <summary>
        /// Extends the IDisposable interface, so that the "using" statement can be used to scope the lifetime of a channel when appropriate.
        /// </summary>
        protected IModel _channel;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public RabbitMQListener(
            IOptions<RabbitMQConnectionConfig> options,
            ILogger<RabbitMQListener> logger)
        {
            RabbitMQConnectionConfig connectionOption = options.Value;

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = connectionOption.HostName,
                    Port = connectionOption.Port,
                    UserName = connectionOption.UserName,
                    Password = connectionOption.Password
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMQ client connection fail");
            }

            _logger = logger;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                const string queueName = "payment_queue";
                var exchange = MQConstants.Exchange;
                Dictionary<string, object> dic = new Dictionary<string, object> { { "x-delayed-type", "direct" } };
                // _channel.ExchangeDeclare(exchange, "direct");
                _channel.ExchangeDeclare(exchange, "x-delayed-message", false, false, dic);
                _channel.QueueDeclare(queueName);
                _channel.QueueBind(queueName, exchange, MQConstants.PaymentTimeOutRouteKey);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (sender, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var result = await Process(e.RoutingKey, message);
                    if (result)
                    {
                        _channel.BasicAck(e.DeliveryTag, false);
                    }
                };

                _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 处理消息方法
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual Task<bool> Process(string routingKey, string message)
        {
            return Task.FromResult(true);
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();

            return Task.CompletedTask;
        }
    }
}