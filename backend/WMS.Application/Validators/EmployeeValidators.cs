using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(x => x.PhoneNumber).MaximumLength(15);
            RuleFor(x => x.Gender).Must(g => string.IsNullOrEmpty(g) || g is "M" or "F" or "O")
                .WithMessage("Gender must be M, F, or O");
            RuleFor(x => x.DateOfBirth).Must(BeAtLeast18YearsOld).WithMessage("Employee must be at least 18 years old");
            RuleFor(x => x.DateOfJoining).NotEmpty();
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.RoleId).GreaterThan(0);
        }

        private bool BeAtLeast18YearsOld(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }
    }

    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50).When(x => x.FirstName != null);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50).When(x => x.LastName != null);
            RuleFor(x => x.PhoneNumber).MaximumLength(15).When(x => x.PhoneNumber != null);
            RuleFor(x => x.Gender).Must(g => g is "M" or "F" or "O")
                .When(x => x.Gender != null)
                .WithMessage("Gender must be M, F, or O");
            RuleFor(x => x.DepartmentId).GreaterThan(0).When(x => x.DepartmentId.HasValue);
            RuleFor(x => x.RoleId).GreaterThan(0).When(x => x.RoleId.HasValue);
        }
    }
}
