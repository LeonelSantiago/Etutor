using Etutor.Core;
using Etutor.DataModel.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Etutor.BL.Abstract
{
    public interface IUsuarioRepository : IEntityBaseRepository<User>
    {
        Task<List<Claim>> GetClaimsAsync(User entity);
        Task<User> FindByNameAsync(string userName);
        Task<User> FindAsync(int id);
        Task AddAsync(User value, string password);
        Task UpdateAsync(User value, string password);
        Task UpdateAsync(User entity);
        Task ChangePasswordAsync(string userName, string currentPassword, string newPassword);
    }
}
