using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class RolClaimEntityConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RolesClaims");

            builder.Property(e => e.Id)
                .HasColumnName("RoleClaimId");

            builder.Property(e => e.RoleId)
                .HasColumnName("RoleId");

            builder.Property(e => e.ClaimType)
                .HasColumnName("Type");

            builder.Property(e => e.ClaimValue)
                .HasColumnName("Value");
        }
    }
}
