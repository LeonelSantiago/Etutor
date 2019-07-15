using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Etutor.Core
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindAsync(int id, params Expression<Func<T, object>>[] includeProperties);
        Task AddAsync(T value);
        void AddRange(IEnumerable<T> values);
        void Update(T value);
        Task RemoveAsync(int key);
    }
}
