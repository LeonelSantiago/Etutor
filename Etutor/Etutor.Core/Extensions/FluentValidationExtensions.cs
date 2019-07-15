using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Etutor.Core.PropertyValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Etutor.Core.Extensions
{
    public static class FluentValidationExtensions
    {
        public static string ToMessage(this IList<ValidationFailure> errors)
        {
            var result = new StringBuilder();
            foreach (var error in errors)
            {
                result.AppendLine(error.ErrorMessage);
            }
            return result.ToString();
        }

        public static Dictionary<string, string> ToMessageDictionary(this IList<ValidationFailure> errors)
        {
            var result = new Dictionary<string, string>();
            foreach (var error in errors)
            {
                result.Add(error.PropertyName, error.ErrorMessage);
            }
            return result;
        }

        public static IRuleBuilderOptions<TItem, TProperty> IsUnique<TItem, TProperty>(
            this IRuleBuilder<TItem, TProperty> ruleBuilder,
            DbContext dbContext,
            IStringLocalizer localizer,
            bool ignoreQueryFilters = false,
            Expression<Func<TItem, object>> uniqueBy = null,
            Expression<Func<TItem, bool>> predicate = null)
            where TItem : class, IEntityBase, new()
        {
            return ruleBuilder.SetValidator(new UniqueValidator<TItem>(dbContext, localizer, ignoreQueryFilters, uniqueBy, predicate));
        }

        public static IRuleBuilderOptions<TItem, TProperty> In<TItem, TProperty>(
            this IRuleBuilder<TItem, TProperty> ruleBuilder,
            IStringLocalizer localizer,
            bool? validateAtInsert = true,
            params TProperty[] validOptions)
            where TItem : class, IEntityBase, new()
        {
            return ruleBuilder.SetValidator(new InPropertyValidator<TItem, TProperty>(localizer, validateAtInsert: validateAtInsert.GetValueOrDefault(), validOptions: validOptions));
        }

        public static IRuleBuilderOptions<TItem, TProperty> InEntityFields<TItem, TProperty>(
           this IRuleBuilder<TItem, TProperty> ruleBuilder,
           IStringLocalizer localizer,
           Type type,
           bool? validateAtInsert = true)
           where TItem : class, IEntityBase, new()
        {
            var propertiesValue = type.GetFields().Select(x => x.GetValue(null));
            return ruleBuilder.SetValidator(new InPropertyValidator<TItem, TProperty>(localizer, validateAtInsert: validateAtInsert.GetValueOrDefault(), validOptions: propertiesValue.Cast<TProperty>().ToArray()));
        }

        public static IRuleBuilderOptions<TItem, TProperty> ScalePrecision<TItem, TProperty>(
            this IRuleBuilder<TItem, TProperty> ruleBuilder,
            int scale, int precision)
            where TItem : class, IEntityBase, new()
        {
            return ruleBuilder.SetValidator(new ScalePrecisionValidator<TItem>(scale, precision));
        }

    }
}
