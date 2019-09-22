using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class RolEntityConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles");

            builder.Property(e => e.Id)
                .HasColumnName("RoleId");

            builder.Property(e => e.Name)
                .HasColumnName("Name");

            builder.Property(e => e.NormalizedName)
                .HasColumnName("NormalizedName");

            builder.Property(e => e.ConcurrencyStamp)
                .HasColumnName("ConcurrencyStamp");
        }
    }
}
