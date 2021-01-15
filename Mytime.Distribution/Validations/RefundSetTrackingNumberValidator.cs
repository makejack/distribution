using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 退货设置快递音速验证器
    /// </summary>
    public class RefundSetTrackingNumberValidator : AbstractValidator<RefundSetTrackingNumberRequest>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RefundSetTrackingNumberValidator()
        {
            RuleFor(x => x.CourierCompanyCode).NotEmpty().WithMessage("快递公司编号不能为空");

            RuleFor(x => x.TrackingNumber).NotEmpty().WithMessage("快递单号不能为空");
        }
    }
}