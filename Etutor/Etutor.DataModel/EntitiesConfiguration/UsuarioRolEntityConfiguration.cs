using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class UsuarioRolEntityConfiguration : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.ToTable("UsuarioRoles", "MGP");

            builder.Property(e => e.UserId)
                .HasColumnName("UsuarioId");

            builder.Property(e => e.RoleId)
                .HasColumnName("RolId");
        }
    }
}
