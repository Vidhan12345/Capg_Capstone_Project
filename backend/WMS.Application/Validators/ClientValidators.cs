using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class CreateClientValidator : AbstractValidator<CreateClientDto>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.ClientName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ClientAddress).MaximumLength(500);
            RuleFor(x => x.ClientPhoneNumber).MaximumLength(20);
            RuleFor(x => x.ClientLocation).MaximumLength(200);
        }
    }

    public class UpdateClientValidator : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientValidator()
        {
            RuleFor(x => x.ClientName).NotEmpty().MaximumLength(200).When(x => x.ClientName != null);
            RuleFor(x => x.ClientAddress).MaximumLength(500).When(x => x.ClientAddress != null);
            RuleFor(x => x.ClientPhoneNumber).MaximumLength(20).When(x => x.ClientPhoneNumber != null);
            RuleFor(x => x.ClientLocation).MaximumLength(200).When(x => x.ClientLocation != null);
        }
    }
}
