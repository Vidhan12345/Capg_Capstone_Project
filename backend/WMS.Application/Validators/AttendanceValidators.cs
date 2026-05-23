using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class CheckInValidator : AbstractValidator<CheckInDto>
    {
        public CheckInValidator()
        {
            RuleFor(x => x.CheckIn).NotEmpty();
            RuleFor(x => x.WorkMode).NotEmpty().Must(m => m is "WFO" or "WFH" or "Hybrid").WithMessage("WorkMode must be WFO, WFH, or Hybrid");
        }
    }

    public class CheckOutValidator : AbstractValidator<CheckOutDto>
    {
        public CheckOutValidator()
        {
            RuleFor(x => x.CheckOut).NotEmpty();
        }
    }
}
