using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 微信用户地址创建验证器
    /// </summary>
    public class CustomerAddressCreateValidator : AbstractValidator<CustomerAddressCreateRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public CustomerAddressCreateValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("请输入收件人姓名").MaximumLength(128).WithMessage("最长128个字符");

            RuleFor(x => x.TelNumber).NotEmpty().WithMessage("请输入收件电话号码");

            RuleFor(x => x.DetailInfo).NotEmpty().WithMessage("请输入详细地址");
        }
    }
}