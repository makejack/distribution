namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 用户团队响应
    /// </summary>
    public class CustomerTeamResponse : CustomerResponse
    {
        /// <summary>
        /// 资产
        /// </summary>
        /// <value></value>
        public AssetsResponse Assets { get; set; }
    }
}