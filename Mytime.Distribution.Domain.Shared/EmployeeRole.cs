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
        /// 仓库管理员
        /// </summary>
        [Description("仓库管理员")]
        Warehouse,
        /// <summary>
        /// 商品管理员
        /// </summary>
        [Description("商品管理员")]
        Goods,
    }
}