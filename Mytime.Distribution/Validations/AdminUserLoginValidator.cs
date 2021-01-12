using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 后台用户登录验证器
    /// </summary>
    public class AdminUserLoginValidator : AbstractValidator<AdminUserLoginRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public AdminUserLoginValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("用户名不能为空").MaximumLength(32).WithMessage("最长32个字符");

            RuleFor(x => x.Pwd).NotEmpty().WithMessage("密码不为空");
            
        }
    }
}