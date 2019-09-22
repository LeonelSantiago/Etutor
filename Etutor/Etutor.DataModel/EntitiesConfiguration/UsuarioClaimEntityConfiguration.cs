using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class UsuarioClaimEntityConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UsersClaims");

            builder.Property(e => e.Id)
                .HasColumnName("UsuarioClaimId");

            builder.Property(e => e.UserId)
                .HasColumnName("UserId");

            builder.Property(e => e.ClaimType)
                .HasColumnName("ClaimType");

            builder.Property(e => e.ClaimValue)
                .HasColumnName("ClaimValue");
        }
    }
}
