using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class ApplyLeaveValidator : AbstractValidator<ApplyLeaveDto>
    {
        public ApplyLeaveValidator()
        {
            RuleFor(x => x.FromDate).NotEmpty();
            RuleFor(x => x.ToDate).NotEmpty();
            RuleFor(x => x).Must(x => x.FromDate <= x.ToDate).WithMessage("FromDate must be before or equal to ToDate");
            RuleFor(x => x).Must(x => x.FromDate >= DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Cannot apply leave for past dates");
            RuleFor(x => x.LeaveType).NotEmpty().Must(BeValidLeaveType).WithMessage("Leave type must be Sick, Casual, or Earned");
            RuleFor(x => x.Reason).MaximumLength(1000);
        }

        private bool BeValidLeaveType(string type)
        {
            return type is "Sick" or "Casual" or "Earned";
        }
    }
}
