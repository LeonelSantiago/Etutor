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
    public class UsuarioLoginEntityConfiguration : IEntityTypeConfiguration<UserLogin>
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

        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            var maxKeyLength = _storeOptions?.MaxLengthForKeys ?? 0;

            builder.ToTable("UsersLogins");

            builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            builder.Property(e => e.LoginProvider)
                .HasColumnName("LoginProvider");

            builder.Property(e => e.ProviderKey)
                .HasColumnName("ProviderKey");

            builder.Property(e => e.ProviderDisplayName)
                .HasColumnName("ProviderDisplayName");

            builder.Property(e => e.UserId)
                .HasColumnName("UserId");

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
