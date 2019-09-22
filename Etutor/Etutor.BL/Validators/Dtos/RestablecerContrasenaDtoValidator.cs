using FluentValidation;
using Microsoft.Extensions.Localization;
using Etutor.BL.Dtos.Identity;
using Etutor.BL.Resources;

namespace Etutor.BL.Validators.Dtos
{
    public class RestablecerContrasenaDtoValidator : AbstractValidator<RestablecerContrasenaDto>
    {
        public RestablecerContrasenaDtoValidator(IStringLocalizer<ShareResource> localizer)
        {
            RuleFor(dto => dto.Password).NotEmpty()
                                    .MaximumLength(256)
                                    .WithName(localizer["Password"]);

            RuleFor(dto => dto.NewPassword).NotEmpty()
                                    .MaximumLength(256)
                                    .WithName(localizer["New Password"]);

            RuleFor(dto => dto.ConfirmPassword).NotEmpty()
                                    .MaximumLength(256)
                                    .Equal(dto => dto.NewPassword)
                                    .WithName(localizer["Confirm Password"]);
        }
    }
}
