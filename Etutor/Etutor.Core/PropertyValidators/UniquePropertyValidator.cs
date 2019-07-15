using FluentValidation.Validators;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Etutor.Core.Extensions;
using Etutor.Core.Models.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Etutor.Core.PropertyValidators
{
    public class UniqueValidator<T> : PropertyValidator where T : class, IEntityBase, new()
    {

        private readonly DbContext _context;
        private readonly bool _ignoreQueryFilters;
        private readonly Expression<Func<T, object>> _uniqueBy;
        private Expression<Func<T, bool>> _predicate;

        public UniqueValidator(
            DbContext context,
            IStringLocalizer localizer,
            bool ignoreQueryFilters,
            Expression<Func<T, object>> uniqueBy = null,
            Expression<Func<T, bool>> predicate = null
            )
            : base(localizer["'{PropertyName}' must be unique, the value already exist."])
        {
            _context = context;
            _ignoreQueryFilters = ignoreQueryFilters;
            _uniqueBy = uniqueBy;
            _predicate = predicate ?? PredicateBuilder.New<T>(true);
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var instance = context.Instance as T ?? new T();
            var queryString = (context.PropertyName + " = " + context.PropertyValue
                        + "| " + "Id" + " != " + instance.Id);

            if (_uniqueBy != null)
                queryString += $"| {(_uniqueBy.Body as MemberExpression ?? ((UnaryExpression)_uniqueBy.Body).Operand as MemberExpression).Member.Name} = {_uniqueBy.Compile()(instance)}";

            var func = queryString.AsPredicate<T>(AndOrOperator.And);

            _predicate = _predicate.And(func);

            IQueryable<T> queryable = _context.Set<T>().Where(_predicate);
            if (_ignoreQueryFilters)
                queryable = queryable.IgnoreQueryFilters();

            return !queryable.Any();
        }
    }
}
