using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 后台分类创建模型验证器
    /// </summary>
    public class AdminCategoryCreateValidator : AbstractValidator<AdminCategoryCreateRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public AdminCategoryCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("类别名称不能为空").MaximumLength(32).WithMessage("最长32个字符");
        }
    }
}