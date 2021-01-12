using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 后台商品选项创建验证器
    /// </summary>
    public class AdminGoodsOptionCreateValidator : AbstractValidator<AdminGoodsOptionCreateRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public AdminGoodsOptionCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空").MaximumLength(32).WithMessage("最长32个字符");

        }
    }
}