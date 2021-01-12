using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台员工创建请求
    /// </summary>
    public class AdminEmployeeCreateRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        /// <value></value>
        public string NickName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        public string Pwd { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        /// <value></value>
        public EmployeeRole Role { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        /// <value></value>
        public string Tel { get; set; }
    }
}