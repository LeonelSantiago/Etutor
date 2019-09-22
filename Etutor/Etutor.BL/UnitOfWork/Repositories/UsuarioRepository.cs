using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Etutor.BL.Abstract;
using Etutor.Core.Exceptions;
using Etutor.Core.Extensions;
using Etutor.DataModel.Context;
using Etutor.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Etutor.BL.UnitOfWork.Repositories
{
    public class UsuarioRepository : EntityBaseRepository<User>, IUsuarioRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<User> _userManager;
        protected readonly IConfiguration _configuration;

        public UsuarioRepository(ApplicationDbContext context,
                                 UserManager<User> userManager,
                                FluentValidation.IValidator<User> validator,
                                IConfiguration configuration)
        : base(context, validator)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }



        public virtual async Task<List<Claim>> GetClaimsAsync(User entity)
        {
            var claims = await _userManager.GetClaimsAsync(entity);
            return claims.ToList();
        }

        public virtual async Task<User> FindByNameAsync(string userName)
        {
            var entity = await _userManager.FindByNameAsync(userName);
            if (entity == null) throw new NotFoundException($"{typeof(User).Name} '{userName}'");
            return entity;
        }

        public virtual async Task<User> FindAsync(int id)
        {
            var entity = await _userManager.FindByIdAsync(id.ToString());
            if (entity == null) throw new NotFoundException($"\"{typeof(User).Name}\" ({id})");
            return entity;
        }

        public virtual async Task AddAsync(User entity, string password)
        {
            var results = _validator.Validate(entity);
            if (!results.IsValid) throw new ValidationException(results.Errors.ToMessage());

            IdentityResult result;

            result = await _userManager.CreateAsync(entity, password);


            if (!result.Succeeded)
                throw new ValidationException(result.Errors.ToMessage());
        }

        public virtual async Task UpdateAsync(User entity, string password)
        {
            var results = _validator.Validate(entity);
            if (!results.IsValid) throw new ValidationException(results.Errors.ToMessage());

            IdentityResult result = IdentityResult.Success;
            if (!password.Equals(_configuration.GetValue<string>("SymbolPasswordRepresentation")))
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(entity);
                result = await _userManager.ResetPasswordAsync(entity, resetToken, password);
            }


            if (!result.Succeeded)
                throw new ValidationException(result.Errors.ToMessage());

            result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.ToMessage());
        }

        public virtual async Task UpdateAsync(User entity)
        {
            var results = _validator.Validate(entity);
            if (!results.IsValid) throw new ValidationException(results.Errors.ToMessage());

            var result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.ToMessage());
        }

        public virtual async Task ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            if (currentPassword.Equals(newPassword))
                throw new ValidationException("The new password can not be the same as the current one.", true);

            var user = await FindByNameAsync(userName);
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors.ToMessage());

            if (!user.LastAccess.HasValue)
            {
                user.LastAccess = DateTime.Now;
                await UpdateAsync(user);
            }
        }
    }
}
