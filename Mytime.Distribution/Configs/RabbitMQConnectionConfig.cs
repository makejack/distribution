namespace Mytime.Distribution.Configs
{
    /// <summary>
    /// rabbitmq 连接配置
    /// </summary>
    public class RabbitMQConnectionConfig
    {
        /// <summary>
        /// RabbitMQ服务地址
        /// </summary>
        /// <value></value>
        public string HostName { get; set; }
        /// <summary>
        /// 服务端口号
        /// </summary>
        /// <value></value>
        public int Port { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        /// <value></value>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        public string Password { get; set; }
    }
}