using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class RolClaimEntityConfiguration : IEntityTypeConfiguration<RolClaim>
    {
        public void Configure(EntityTypeBuilder<RolClaim> builder)
        {
            builder.ToTable("RolClaims", "MGP");

            builder.Property(e => e.Id)
                .HasColumnName("RolClaimId");

            builder.Property(e => e.RoleId)
                .HasColumnName("RolId");

            builder.Property(e => e.ClaimType)
                .HasColumnName("Tipo");

            builder.Property(e => e.ClaimValue)
                .HasColumnName("Valor");
        }
    }
}
