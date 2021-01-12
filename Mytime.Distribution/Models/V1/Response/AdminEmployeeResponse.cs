using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台员工响应
    /// </summary>
    public class AdminEmployeeResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
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
        /// 角色
        /// </summary>
        /// <value></value>
        public EmployeeRole Role { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        /// <value></value>
        public string Tel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}