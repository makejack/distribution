using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 后台商品选项数据编辑验证器
    /// </summary>
    public class AdminGoodsOptionDataEditValidator : AbstractValidator<AdminGoodsOptionDataEditRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public AdminGoodsOptionDataEditValidator()
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("值不能为空").MaximumLength(32).WithMessage("最大32个字符");

            RuleFor(x => x.Description).MaximumLength(512).WithMessage("最大512个字符");
        }
    }
}