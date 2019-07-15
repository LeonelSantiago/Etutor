using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class RolEntityConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles", "MGP");

            builder.Property(e => e.Id)
                .HasColumnName("RolId");

            builder.Property(e => e.Name)
                .HasColumnName("Nombre");

            builder.Property(e => e.NormalizedName)
                .HasColumnName("NombreNormalizado");

            builder.Property(e => e.ConcurrencyStamp)
                .HasColumnName("SelloConcurrencia");
        }
    }
}
