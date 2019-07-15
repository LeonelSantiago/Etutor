using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Etutor.Core.Extensions;
using Etutor.Core.Models.Configurations;
using Etutor.DataModel.Context;
using Etutor.DataModel.Entities;
using System;
using Etutor.BL.Resources;

namespace Etutor.BL.Validators
{
    public class UsuarioValidator : AbstractValidatorBase<Usuario>
    {
        public UsuarioValidator(ApplicationDbContext context,
                                IStringLocalizer<ShareResource> localizer,
                                IOptions<AdConfig> config) : base(localizer)
        {
            RuleFor(entity => entity.Nombre).NotEmpty()
                .MaximumLength(256)
                .WithName(localizer["Name"]);

            RuleFor(entity => entity.Apellido).NotEmpty()
                .MaximumLength(256)
                .WithName(localizer["Last Name"]);

            RuleFor(entity => entity.UserName).NotEmpty()
                .MaximumLength(256)
                .IsUnique(context, localizer, ignoreQueryFilters: true)
                .WithName(localizer["User Name"]);

            RuleFor(entity => entity.Email)
                .Must(email => MatchDomainName(email, config.Value.DomainName))
                    .WithMessage(string.Format(localizer["'{0}' must have domain name equal to {1}."], localizer["Email"], config.Value.DomainName))
                .NotEmpty()
                .MaximumLength(256)
                .EmailAddress()
                .IsUnique(context, localizer, ignoreQueryFilters: true)
                .WithName(localizer["Email"]);
        }

        public bool MatchDomainName(string email, string specifiedDomainName)
        {
            var split = email.Split("@", StringSplitOptions.RemoveEmptyEntries);
            var domain = split.Length > 1 ? split[1] : string.Empty;
            return domain.Equals(specifiedDomainName);
        }
    }
}
