using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class UsuarioRolEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UsersRoles");

            builder.Property(e => e.UserId)
                .HasColumnName("UserId");

            builder.Property(e => e.RoleId)
                .HasColumnName("RoleId");
        }
    }
}
