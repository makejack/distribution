using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;

namespace Mytime.Distribution.Filters
{
    /// <summary>
    /// 后台用户注册过滤属性
    /// </summary>
    public class AdminUserRegisterFilterAttribute : ActionFilterAttribute
    {
        private IRepositoryByInt<AdminUser> _adminUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adminUserRepository"></param>
        public AdminUserRegisterFilterAttribute(IRepositoryByInt<AdminUser> adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var quantity = _adminUserRepository.Query().Count();
            if (quantity > 0)
            {
                context.Result = new ObjectResult(Result.Fail(ResultCodes.NotRegister));
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}