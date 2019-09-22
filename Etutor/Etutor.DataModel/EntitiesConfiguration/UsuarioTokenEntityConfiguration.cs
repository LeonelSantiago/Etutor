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
    public class UsuarioTokenEntityConfiguration : IEntityTypeConfiguration<UserToken>
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
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            var maxKeyLength = _storeOptions?.MaxLengthForKeys ?? 0;
            var encryptPersonalData = _storeOptions?.ProtectPersonalData ?? false;

            builder.ToTable("UsersTokens");

            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            builder.Property(e => e.UserId)
                .HasColumnName("UserId");

            builder.Property(e => e.LoginProvider)
                .HasColumnName("LoginProvider");

            builder.Property(e => e.Name)
                .HasColumnName("Name");

            builder.Property(e => e.Value)
                .HasColumnName("Value");

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
                var tokenProps = typeof(UserToken).GetProperties().Where(
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
