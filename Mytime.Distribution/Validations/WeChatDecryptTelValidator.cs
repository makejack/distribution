using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 微信解密手机号验证器
    /// </summary>
    public class WeChatDecryptTelValidator : AbstractValidator<WeChatDecryptTelRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public WeChatDecryptTelValidator()
        {
            RuleFor(e => e.Iv).NotEmpty().WithMessage("Iv不能为空");

            RuleFor(e => e.EncryptedData).NotEmpty().WithMessage("EncryptedData不能为空");
        }
    }
}