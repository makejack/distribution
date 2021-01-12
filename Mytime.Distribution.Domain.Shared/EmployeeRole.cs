using System.ComponentModel;

namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 员工角色
    /// </summary>
    public enum EmployeeRole : byte
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin,
        /// <summary>
        /// 账务
        /// </summary>
        [Description("账务")]
        Accounting,
        /// <summary>
        /// 开发人员
        /// </summary>
        [Description("开发人员")]
        Development,
        /// <summary>
        /// 售后人员
        /// </summary>
        [Description("售后人员")]
        AfterSale,
    }
}