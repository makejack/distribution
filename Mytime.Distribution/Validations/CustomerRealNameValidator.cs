using System.Data;
using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 用户实名验证器
    /// </summary>
    public class CustomerRealNameValidator : AbstractValidator<CustomerRealNameRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public CustomerRealNameValidator()
        {
            RuleFor(e => e.Name).NotEmpty().WithMessage("姓名不能为空");

            RuleFor(e => e.PhoneNumber).NotEmpty().WithMessage("手机号不能为空");

            RuleFor(e => e.BankCode).NotEmpty().WithMessage("银行编号不能为空");

            RuleFor(e => e.BankNo).NotEmpty().WithMessage("银行卡编号不能为空");
        }
    }
}