using Etutor.Core;
using Etutor.DataModel.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Etutor.BL.Abstract
{
    public interface IUsuarioRepository : IEntityBaseRepository<Usuario>
    {
        Task<List<Claim>> GetClaimsAsync(Usuario entity);
        Task<Usuario> FindByNameAsync(string userName);
        Task<Usuario> FindAsync(int id);
        Task AddAsync(Usuario value, string password);
        Task UpdateAsync(Usuario value, string password);
        Task UpdateAsync(Usuario entity);
        Task ChangePasswordAsync(string userName, string currentPassword, string newPassword);
    }
}
