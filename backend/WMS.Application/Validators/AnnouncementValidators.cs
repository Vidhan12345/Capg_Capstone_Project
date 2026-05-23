using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators
{
    public class CreateAnnouncementValidator : AbstractValidator<CreateAnnouncementDto>
    {
        public CreateAnnouncementValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(5000);
        }
    }

    public class UpdateAnnouncementValidator : AbstractValidator<UpdateAnnouncementDto>
    {
        public UpdateAnnouncementValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200).When(x => x.Title != null);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(5000).When(x => x.Content != null);
        }
    }
}