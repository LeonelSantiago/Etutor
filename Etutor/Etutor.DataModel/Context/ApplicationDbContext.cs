using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Etutor.Core;
using Etutor.Core.Extensions;
using Etutor.Core.Models;
using Etutor.DataModel.Entities;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Etutor.DataModel.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Rol, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        #region OnModelCreating
        /// <summary>
        /// Override OnModelCreating so we can perform operations on the ModelBuilder object.
        /// </summary>
        /// <returns></returns>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // NOTE: Through reflection, all the classes that implement 
            // "IEntityTypeConfiguration <>" are read and then they are registered 
            // to be created by Entity Framework.
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                                  .Where(x => x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))))
            {
                var hasConstructorParams = type.GetConstructors().Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(ApplicationDbContext)));
                dynamic configurationInstance = hasConstructorParams ? Activator.CreateInstance(type, this) : Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            // NOTE: A query filter is applied to all the entities that inherit from 
            // IEntidadAuditableBase with the objective that when a result of the 
            // database is obtained, it automatically ignores the records that have 
            // the true value in "Borrado" field.
            foreach (var type in modelBuilder.Model.GetEntityTypes()
                                .Where(type => typeof(IEntityAuditableBase).IsAssignableFrom(type.ClrType)))
            {
                modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
        }
        #endregion

        #region SaveChanges
        /// <summary>
        /// Override SaveChanges so we can call the new AuditEntities method.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            AuditEntities();

            return base.SaveChanges();
        }
        #endregion

        #region SaveChangesAsync
        /// <summary>
        /// Override SaveChanges so we can call the new AuditEntities method.
        /// </summary>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AuditEntities();

            return base.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region AuditEntities
        /// <summary>
        /// Method that will set the Audit properties for every added or modified Entity marked with the 
        /// IAuditable interface.
        /// </summary>
        private void AuditEntities()
        {

            // Get the authenticated user name 
            int userId = 0;

            var user = ClaimsPrincipal.Current;  /*Thread.CurrentPrincipal;*/
            if (user != null)
            {
                var identity = user.Identity;
                if (identity != null)
                {
                    //userId = identity.Name;
                }
            }

            // For every changed entity marked as "IEntidadAuditableBase" set the values for the audit properties
            foreach (EntityEntry<IEntityAuditableBase> entry in ChangeTracker.Entries<IEntityAuditableBase>())
            {
                if (entry.State == EntityState.Added) // If the entity was added.
                {
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.Status = EntityStatus.Active;
                }
                else if (entry.State == EntityState.Modified) // If the entity was updated
                {
                    entry.Entity.ModifiedBy = userId;
                    entry.Entity.ModifiedAt = DateTime.Now;
                    Entry(entry.Entity).Property(x => x.CreatedAt).IsModified = false;
                    Entry(entry.Entity).Property(x => x.CreatedBy).IsModified = false;
                }
            }
        }
        #endregion
    }
}
