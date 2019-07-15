using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Etutor.DataModel.Entities;
using System.Linq;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class UsuarioLoginEntityConfiguration : IEntityTypeConfiguration<UsuarioLogin>
    {
        private readonly StoreOptions _storeOptions;

        public UsuarioLoginEntityConfiguration(Context.ApplicationDbContext context)
        {
            _storeOptions = context.GetService<IDbContextOptions>()
                    .Extensions.OfType<CoreOptionsExtension>()
                    .FirstOrDefault()?.ApplicationServiceProvider
                    ?.GetService<IOptions<IdentityOptions>>()
                    ?.Value?.Stores;
        }

        public void Configure(EntityTypeBuilder<UsuarioLogin> builder)
        {
            var maxKeyLength = _storeOptions?.MaxLengthForKeys ?? 0;

            builder.ToTable("UsuarioLogins", "MA");

            builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            builder.Property(e => e.LoginProvider)
                .HasColumnName("ProveedorLogin");

            builder.Property(e => e.ProviderKey)
                .HasColumnName("ClaveProveedor");

            builder.Property(e => e.ProviderDisplayName)
                .HasColumnName("NombreProveedor");

            builder.Property(e => e.UserId)
                .HasColumnName("UsuarioId");

            if (maxKeyLength > 0)
            {
                builder.Property(l => l.LoginProvider)
                    .HasMaxLength(maxKeyLength);
                builder.Property(l => l.ProviderKey)
                    .HasMaxLength(maxKeyLength);
            }
        }
    }
}
