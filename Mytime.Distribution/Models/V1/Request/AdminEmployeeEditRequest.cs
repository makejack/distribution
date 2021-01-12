using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台员工编辑请求
    /// </summary>
    public class AdminEmployeeEditRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        /// <value></value>
        public string NickName { get; set; }
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