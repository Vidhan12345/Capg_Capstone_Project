using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }

    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).When(x => x.Name != null);
            RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
        }
    }
}
