using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.Status).Must(s => s is "NotStarted" or "InProgress" or "Completed" or "OnHold")
                .WithMessage("Status must be NotStarted, InProgress, Completed, or OnHold");
        }
    }

    public class UpdateProjectValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200).When(x => x.Name != null);
            RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description != null);
            RuleFor(x => x.StartDate).NotEmpty().When(x => x.StartDate.HasValue);
            RuleFor(x => x.ClientId).GreaterThan(0).When(x => x.ClientId.HasValue);
            RuleFor(x => x.Status).Must(s => s is "NotStarted" or "InProgress" or "Completed" or "OnHold")
                .When(x => x.Status != null)
                .WithMessage("Status must be NotStarted, InProgress, Completed, or OnHold");
        }
    }
}
