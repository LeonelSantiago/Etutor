using Microsoft.EntityFrameworkCore;
using Etutor.BL.Abstract;
using Etutor.BL.UnitOfWork.Repositories;
using Etutor.Core;
using Etutor.DataModel.Context;
using StructureMap;
using System.Threading.Tasks;

namespace Etutor.BL.UnitOfWork
{
    public partial class UnitOfWork
    {
        private readonly IContainer _container;

        public UnitOfWork(IContainer container)
        {
            _container = container;
        }

        public IEntityBaseRepository<TEntity> Get<TEntity>() where TEntity : class, IEntityBase, new()
        {
            return _container.GetInstance<IEntityBaseRepository<TEntity>>();
        }
        public async Task SaveAsync()
        {
            try
            {
                await _container.GetInstance<ApplicationDbContext>().SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
                //Log the error (uncomment ex variable name and write a log.)
                //ModelState.AddModelError("", "Unable to save changes. " +
                //"Try again, and if the problem persists, " +
                // "see your system administrator.");
            }
        }

        private IUsuarioRepository _usuarioRepository;
        public IUsuarioRepository UsuarioRepository => _usuarioRepository ?? (_usuarioRepository = _container.GetInstance<UsuarioRepository>());
    }
}
