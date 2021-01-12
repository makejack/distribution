using System.Data;
using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 后台商品创建验证器
    /// </summary>
    public class AdminGoodsCreateValidator : AbstractValidator<AdminGoodsCreateRequest>
    {
        /// <summary>
        /// 验证器
        /// </summary>
        public AdminGoodsCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("商品名称不能为空").MaximumLength(512).WithMessage("最长512个字符");
        }
    }
}