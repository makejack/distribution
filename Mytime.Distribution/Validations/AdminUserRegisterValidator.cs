using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 后台用户注册验证器
    /// </summary>
    public class AdminUserRegisterValidator : AbstractValidator<AdminUserRegisterRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public AdminUserRegisterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("用户名不能为空").MaximumLength(32).WithMessage("最长32个字符");

            RuleFor(x => x.Pwd).NotEmpty().WithMessage("密码不为空").Length(6, 32).WithMessage("密码长度必须符合规则（6-32）位");
        }
    }
}