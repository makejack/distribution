namespace Mytime.Distribution.Configs
{
    /// <summary>
    /// 授权配置
    /// </summary>
    public class AuthenticationConfig
    {
        /// <summary>
        /// Key
        /// </summary>
        /// <value></value>
        public string Key { get; set; }
        /// <summary>
        /// Issuer
        /// </summary>
        /// <value></value>
        public string Issuer { get; set; }
        /// <summary>
        /// Audience
        /// </summary>
        /// <value></value>
        public string Audience { get; set; }
        /// <summary>
        /// Expiress
        /// </summary>
        /// <value></value>
        public int Expiress { get; set; }
    }
}