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
    public class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
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
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            var encryptPersonalData = _storeOptions?.ProtectPersonalData ?? false;

            builder.ToTable("Usuarios", "MA");

            builder.Property(e => e.Id)
                .HasColumnName("UsuarioId");

            builder.HasIndex(u => u.NormalizedUserName)
                .HasName("IX_Nombre_Usuario")
                .IsUnique();

            builder.HasIndex(u => u.NormalizedEmail)
                .HasName("IX_Correo")
                .IsUnique();

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            builder.Property(e => e.Apellido)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            builder.Property(e => e.Estado)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(false)
                .HasDefaultValueSql("('A')");

            builder.Property(e => e.UserName)
                 .HasMaxLength(256)
                 .HasColumnName("NombreUsuario");

            builder.Property(e => e.NormalizedUserName)
                 .HasMaxLength(256)
                 .HasColumnName("NombreUsuarioNomalizado");

            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("Correo");

            builder.Property(e => e.NormalizedEmail)
                 .HasMaxLength(256)
                 .HasColumnName("CorreoNormalizado");

            builder.Property(e => e.PasswordHash)
                .HasColumnName("ContrasenaHash");

            builder.Property(e => e.SecurityStamp)
                .HasColumnName("SelloSeguridad");

            builder.Property(e => e.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasColumnName("SelloConcurrencia");

            builder.Property(e => e.PhoneNumber)
                .HasColumnName("Celular");

            builder.Property(e => e.PhoneNumberConfirmed)
                .HasColumnName("CelularConfirmado");

            builder.Property(e => e.TwoFactorEnabled)
                .HasColumnName("DosFactoresHabilitado");

            builder.Property(e => e.LockoutEnd)
                .HasColumnName("BloqueoFinal");

            builder.Property(e => e.LockoutEnabled)
                .HasColumnName("BloqueoHabilitado");

            builder.Property(e => e.AccessFailedCount)
                .HasColumnName("ConteoAccesoFallido");

            builder.HasMany<UsuarioClaim>()
                .WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            builder.HasMany<UsuarioLogin>()
                .WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            builder.HasMany<UsuarioToken>()
                .WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            if (encryptPersonalData)
            {
                var converter = new PersonalDataConverter(_context.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(Usuario).GetProperties().Where(
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
