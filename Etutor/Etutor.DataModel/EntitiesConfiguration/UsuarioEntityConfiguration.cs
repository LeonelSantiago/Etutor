using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Etutor.DataModel.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.DataConverter;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class UsuarioEntityConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly StoreOptions _storeOptions;
        private readonly Context.ApplicationDbContext _context;

        public UsuarioEntityConfiguration(Context.ApplicationDbContext context)
        {
            _storeOptions = context.GetService<IDbContextOptions>()
                        .Extensions.OfType<CoreOptionsExtension>()
                        .FirstOrDefault()?.ApplicationServiceProvider
                        ?.GetService<IOptions<IdentityOptions>>()
                        ?.Value?.Stores;
            _context = context;
        }
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var encryptPersonalData = _storeOptions?.ProtectPersonalData ?? false;

            builder.ToTable("Users");

            builder.Property(e => e.Id)
                .HasColumnName("Id");

            builder.HasIndex(u => u.NormalizedUserName)
                .HasName("IX_Name_User")
                .IsUnique();

            builder.HasIndex(u => u.NormalizedEmail)
                .HasName("IX_Email")
                .IsUnique();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue(1) 
                .HasColumnName("Status");

            builder.Property(e => e.UserName)
                 .HasMaxLength(256)
                 .HasColumnName("UserName");

            builder.Property(e => e.NormalizedUserName)
                 .HasMaxLength(256)
                 .HasColumnName("NormalizedUserName");

            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("Email");

            builder.Property(e => e.NormalizedEmail)
                 .HasMaxLength(256)
                 .HasColumnName("NormalizedEmail");

            builder.Property(e => e.PasswordHash)
                .HasColumnName("PasswordHash");

            builder.Property(e => e.SecurityStamp)
                .HasColumnName("SecurityStamp");

            builder.Property(e => e.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasColumnName("ConcurrencyStamp");

            builder.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber");

            builder.Property(e => e.PhoneNumberConfirmed)
                .HasColumnName("PhoneNumberConfirmed");

            builder.Property(e => e.TwoFactorEnabled)
                .HasColumnName("TwoFactorEnabled");

            builder.Property(e => e.LockoutEnd)
                .HasColumnName("LockoutEnd");

            builder.Property(e => e.LockoutEnabled)
                .HasColumnName("LockoutEnabled");

            builder.Property(e => e.AccessFailedCount)
                .HasColumnName("AccessFailedCount");

            builder.HasMany<UserClaim>()
                .WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            builder.HasMany<UserLogin>()
                .WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            builder.HasMany<UserToken>()
                .WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            if (encryptPersonalData)
            {
                var converter = new PersonalDataConverter(_context.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(User).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                foreach (var p in personalDataProps)
                {
                    if (p.PropertyType != typeof(string))
                        throw new InvalidOperationException("Can Only Protect Strings");
                    builder.Property(typeof(string), p.Name).HasConversion(converter);
                }
            }
        }
    }
}
