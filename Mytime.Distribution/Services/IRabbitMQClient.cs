using System.Collections.Generic;
using RabbitMQ.Client;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// RabbitMq客户端
    /// </summary>
    public interface IRabbitMQClient
    {
        /// <summary>
        /// 创建属性
        /// </summary>
        /// <returns></returns>
        IBasicProperties CreateProperties();
        /// <summary>
        /// 路由模式发送
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        /// <param name="Tmessage"></param>
        /// <typeparam name="T"></typeparam>
        void PushMessage<T>(string exchange, string routeKey, T Tmessage);
        /// <summary>
        /// 路由模式发送
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        /// <param name="Tmessage"></param>
        /// <param name="properties"></param>
        /// <typeparam name="T"></typeparam>
        void PushMessage<T>(string exchange, string routeKey, T Tmessage, IBasicProperties properties = null);
    }
}