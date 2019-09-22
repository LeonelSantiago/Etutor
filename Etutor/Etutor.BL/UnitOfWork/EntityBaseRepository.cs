using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Etutor.Core.Extensions;
using Etutor.Core.Exceptions;
using Etutor.Core;

namespace Etutor.BL.UnitOfWork
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private readonly DbContext _context;
        public DbSet<T> _set;
        public FluentValidation.IValidator<T> _validator;

        public EntityBaseRepository(DbContext context,
                                    FluentValidation.IValidator<T> validator)
        {
            _context = context;
            _set = context.Set<T>();
            _validator = validator;
        }

        public virtual async Task AddAsync(T entity)
        {
            var results = _validator.Validate(entity);
            if (!results.IsValid) throw new ValidationException(results.Errors.ToMessage());

            await _set.AddAsync(entity);
        }

        public virtual void AddRange(IEnumerable<T> entityEnumerable)
        {
            foreach (var ent in entityEnumerable)
            {
                var results = _validator.Validate(ent);
                if (!results.IsValid) throw new ValidationException(results.Errors.ToMessage());
            }
            _set.AddRange(entityEnumerable);
        }

        public virtual async Task<T> FindAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _set.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var entity = await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) throw new NotFoundException($"\"{typeof(T).Name}\" ({id})");

            return entity;
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _set.Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsNoTracking();
        }

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _set.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual async Task RemoveAsync(int key)
        {
            var entity = await FindAsync(key);

            EntityEntry dbEntityEntry = _context.Entry(entity);
            entity.IsDeleted = true;
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Update(T entity)
        {
            var results = _validator.Validate(entity);
            if (!results.IsValid) throw new ValidationException(results.Errors.ToMessage());

            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
    }
}
