using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Etutor.DataModel.Entities;
using System.Linq;
using Etutor.DataModel.DataConverter;
using System;


namespace Etutor.DataModel.EntitiesConfiguration
{
    public class UsuarioTokenEntityConfiguration : IEntityTypeConfiguration<UsuarioToken>
    {
        private readonly StoreOptions _storeOptions;
        private readonly Context.ApplicationDbContext _context;
        public UsuarioTokenEntityConfiguration(Context.ApplicationDbContext context)
        {
            _storeOptions = context.GetService<IDbContextOptions>()
                        .Extensions.OfType<CoreOptionsExtension>()
                        .FirstOrDefault()?.ApplicationServiceProvider
                        ?.GetService<IOptions<IdentityOptions>>()
                        ?.Value?.Stores;
            _context = context;
        }
        public void Configure(EntityTypeBuilder<UsuarioToken> builder)
        {
            var maxKeyLength = _storeOptions?.MaxLengthForKeys ?? 0;
            var encryptPersonalData = _storeOptions?.ProtectPersonalData ?? false;

            builder.ToTable("UsuarioTokens", "MA");

            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            builder.Property(e => e.UserId)
                .HasColumnName("UsuarioId");

            builder.Property(e => e.LoginProvider)
                .HasColumnName("ProveedorLogin");

            builder.Property(e => e.Name)
                .HasColumnName("Nombre");

            builder.Property(e => e.Value)
                .HasColumnName("Valor");

            if (maxKeyLength > 0)
            {
                builder.Property(t => t.LoginProvider)
                    .HasMaxLength(maxKeyLength);
                builder.Property(t => t.Name)
                    .HasMaxLength(maxKeyLength);
            }

            if (encryptPersonalData)
            {
                var converter = new PersonalDataConverter(_context.GetService<IPersonalDataProtector>());
                var tokenProps = typeof(UsuarioToken).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                foreach (var p in tokenProps)
                {
                    if (p.PropertyType != typeof(string))
                        throw new InvalidOperationException("Can Only Protect Strings");

                    builder.Property(typeof(string), p.Name).HasConversion(converter);
                }
            }
        }
    }
}
