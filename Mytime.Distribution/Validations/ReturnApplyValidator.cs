using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 退货申请验证器
    /// </summary>
    public class ReturnApplyValidator : AbstractValidator<ReturnApplyRequest>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnApplyValidator()
        {
            RuleFor(x => x.Reason).NotEmpty().WithMessage("原因不能为空")
            .MaximumLength(256).WithMessage("最长256个字符");
        }
    }
}