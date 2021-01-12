using FluentValidation;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Validations
{
    /// <summary>
    /// 设置快递单号验证器
    /// </summary>
    public class AdminShipmentSetTrackingNumberValidator : AbstractValidator<AdminShipmentSetTrackingNumberRequest>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdminShipmentSetTrackingNumberValidator()
        {
            RuleFor(x => x.CourierCompany).NotEmpty().WithMessage("快递公司名称不能为空");

            RuleFor(x => x.CourierCompanyCode).NotEmpty().WithMessage("快递公司编码不能为空");

            RuleFor(x => x.TrackingNumber).NotEmpty().WithMessage("快递单号不能为空");
        }
    }
}