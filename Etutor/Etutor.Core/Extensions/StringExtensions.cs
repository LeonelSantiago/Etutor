using LinqKit;
using Etutor.Core.Models.Enums;
using Etutor.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Etutor.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToTittleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(value);
        }

        public static string PascalToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }

        public static Expression<Func<T, bool>> AsPredicate<T>(this string filterString, AndOrOperator andOrOperator = AndOrOperator.Or) where T : IEntityBase
        {
            var predicate = PredicateBuilder.New<T>();
            var param = Expression.Parameter(typeof(T), "x");
            var list = new List<Expression>();
            var queryparts = filterString.Split('|');
            //

            foreach (var part in queryparts)
            {
                var elements = part.Trim().Split(' ');
                //
                string value = null;
                var propertie = elements[0];
                try
                {
                    value = part.Trim().Split(elements[1])[1].Trim();
                }
                catch
                {
                    value = null;
                }

                var expreTest = ExpressionParser.ToExpression2<T>(param, propertie, elements[1], value);
                if (expreTest != null)
                    list.Add(expreTest);

            }

            if (list.Count <= 0) return predicate;
            var tmp = list.Aggregate((expression, expression1) => andOrOperator == AndOrOperator.Or
                ? Expression.Or(expression, expression1)
                : Expression.And(expression, expression1));
            predicate.And(Expression.Lambda<Func<T, bool>>(tmp, param));

            return predicate;
        }
    }
}
