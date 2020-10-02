using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Etutor.BL.Abstract;
using Etutor.DataModel.Context;
using Etutor.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Etutor.Core.Exceptions;
using Etutor.Core.Extensions;

namespace Etutor.BL.UnitOfWork.Repositories
{
    public class EventsRepository : EntityBaseRepository<Events>, IEventsRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IConfiguration _configuration;

        public EventsRepository(ApplicationDbContext context,
                                FluentValidation.IValidator<Events> validator,
                                IConfiguration configuration)
        : base(context, validator)
        {
            _context = context;
            _configuration = configuration;
        }

        public virtual async Task AddAsync(Events events)
        {
            var results = _validator.Validate(events);
            if (!results.IsValid) throw new ValidationException(results.Errors.ToMessage());


            result = await _context.Users


            if (!result.Succeeded)
                throw new ValidationException(result.Errors.ToMessage());
        }

        public void AddRange(IEnumerable<Events> values)
        {
            throw new NotImplementedException();
        }

        public Task<Events> FindAsync(int id, params Expression<Func<Events, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Events> FindBy(Expression<Func<Events, bool>> predicate, params Expression<Func<Events, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Events> GetAll(params Expression<Func<Events, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int key)
        {
            throw new NotImplementedException();
        }

        public void Update(Events value)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Events events)
        {
            throw new NotImplementedException();
        }

        Task<Events> IEventsRepository.FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
