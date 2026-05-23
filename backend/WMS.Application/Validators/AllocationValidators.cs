using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class CreateAllocationValidator : AbstractValidator<CreateAllocationDto>
    {
        public CreateAllocationValidator()
        {
            RuleFor(x => x.EmployeeId).GreaterThan(0);
            RuleFor(x => x.ProjectId).GreaterThan(0);
            RuleFor(x => x.RoleOnProject).MaximumLength(100);
        }
    }
}