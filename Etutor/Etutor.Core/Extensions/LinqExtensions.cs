using System;
using System.Linq;
using System.Linq.Expressions;

namespace Etutor.Core.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> GetByPredicateKey<Key, T>(this IQueryable<T> queryable, Key primaryKeyValue, Expression<Func<T, Key>> predicate)
            where T : class, new()
        {
            ParameterExpression entity = Expression.Parameter(typeof(T), "ent");
            Expression keyValue = Expression.Invoke(predicate, entity);
            Expression pkValue = Expression.Constant(primaryKeyValue, keyValue.Type);
            Expression body = Expression.Equal(keyValue, pkValue);
            return queryable.Where(Expression.Lambda<Func<T, bool>>(body, entity));
        }
    }
}
