namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台小程序设置保存请求
    /// </summary>
    public class AdminLiteAppSaveRequest
    {
        /// <summary>
        /// 城市权益
        /// </summary>
        /// <value></value>
        public string CityMembershipRights { get; set; }

        /// <summary>
        /// 网点权益
        /// </summary>
        /// <value></value>
        public string BranchMembershipRights { get; set; }

    }
}