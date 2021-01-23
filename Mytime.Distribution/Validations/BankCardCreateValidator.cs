using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 创建银行卡验证器
    /// </summary>
    public class BankCardCreateValidator : AbstractValidator<BankCardCreateRequest>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BankCardCreateValidator()
        {
            RuleFor(e => e.Name).NotEmpty().WithMessage("姓名不能为空");

            RuleFor(e => e.PhoneNumber).NotEmpty().WithMessage("手机号不能为空");

            RuleFor(e => e.BankCode).NotEmpty().WithMessage("银行编号不能为空");

            RuleFor(e => e.BankNo).NotEmpty().WithMessage("银行卡编号不能为空")
            .Matches(@"^([1-9]{1})(\d{14}|\d{18})$").WithMessage("银行卡编号格式不正确");
        }
    }
}