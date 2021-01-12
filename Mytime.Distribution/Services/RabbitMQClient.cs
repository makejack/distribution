using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Configs;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// RabbitMq客户端
    /// </summary>
    public class RabbitMQClient : IRabbitMQClient
    {
        private readonly ILogger _logger;
        private readonly IModel _channcel;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="logger"></param>
        public RabbitMQClient(IOptions<RabbitMQConnectionConfig> option, ILogger<RabbitMQClient> logger)
        {

            RabbitMQConnectionConfig connectionOption = option.Value;

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = connectionOption.HostName,
                    Port = connectionOption.Port,
                    UserName = connectionOption.UserName,
                    Password = connectionOption.Password
                };

                var connection = factory.CreateConnection();
                _channcel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMQ client connection fail");
            }
            _logger = logger;
        }

        /// <summary>
        /// 路由模式发送
        /// </summary>
        /// <param name="exchange">交换器</param>
        /// <param name="routeKey"></param>
        /// <param name="Tmessage"></param>
        /// <typeparam name="T"></typeparam>
        public void PushMessage<T>(string exchange, string routeKey, T Tmessage)
        {
            this.PushMessage(exchange, routeKey, Tmessage, null);
        }

        /// <summary>
        /// 路由模式发送
        /// </summary>
        /// <param name="exchange">交换器</param>
        /// <param name="routeKey"></param>
        /// <param name="Tmessage"></param>
        /// <param name="properties"></param>
        /// <typeparam name="T"></typeparam>
        public void PushMessage<T>(string exchange, string routeKey, T Tmessage, IBasicProperties properties = null)
        {
            // _channcel.ExchangeDeclare(exchange, "direct");

            var msgJson = JsonConvert.SerializeObject(Tmessage);
            var body = Encoding.UTF8.GetBytes(msgJson);

            _channcel.BasicPublish(exchange, routeKey, false, properties, body);
        }

        /// <summary>
        /// 创建属性
        /// </summary>
        /// <returns></returns>
        public IBasicProperties CreateProperties()
        {
            return _channcel.CreateBasicProperties();
        }
    }
}