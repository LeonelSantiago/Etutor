using FluentValidation;
using Microsoft.Extensions.Localization;
using Etutor.BL.Dtos.Identity;
using Etutor.BL.Resources;

namespace Etutor.BL.Validators.Dtos
{
    public class InicioSesionDtoValidator : AbstractValidator<LoginDto>
    {
        public InicioSesionDtoValidator(IStringLocalizer<ShareResource> localizer)
        {
            RuleFor(dto => dto.User).NotEmpty()
                                .MaximumLength(256)
                                .WithName(localizer["User name"]);

            RuleFor(dto => dto.Password).NotEmpty()
                                .MaximumLength(256)
                                .WithName(localizer["Password"]);
        }
    }
}
