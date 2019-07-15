using FluentValidation;
using Microsoft.Extensions.Localization;
using Etutor.BL.Resources;
using Etutor.Core;
using Etutor.Core.Extensions;
using Etutor.Core.Models;

namespace Etutor.BL.Validators
{
    public abstract class AbstractValidatorBase<T> : AbstractValidator<T> where T : class, IEntityAuditableBase, new()
    {
        public AbstractValidatorBase(IStringLocalizer<ShareResource> localizer)
        {
            //validaciones genéricas
            RuleFor(entity => entity.Estado).InEntityFields(localizer, typeof(EntityStatus), false)
                .WithName(localizer["Status"]);
        }
    }
}
