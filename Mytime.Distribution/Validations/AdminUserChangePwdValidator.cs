using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 后台用户修改密码验证器
    /// </summary>
    public class AdminUserChangePwdValidator : AbstractValidator<AdminUserChangePwdRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public AdminUserChangePwdValidator()
        {
            RuleFor(x => x.OldPwd).NotEmpty().WithMessage("旧密码不能为空").MaximumLength(32).WithMessage("最大32个字符");

            RuleFor(x => x.NewPwd).NotEmpty().WithMessage("新密码不能为空").Length(6, 32).WithMessage("密码长度6-32个字符");
        }
    }
}