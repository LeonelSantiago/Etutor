using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Etutor.DataModel.Entities;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class UsuarioClaimEntityConfiguration : IEntityTypeConfiguration<UsuarioClaim>
    {
        public void Configure(EntityTypeBuilder<UsuarioClaim> builder)
        {
            builder.ToTable("UsuarioClaims", "MA");

            builder.Property(e => e.Id)
                .HasColumnName("UsuarioClaimId");

            builder.Property(e => e.UserId)
                .HasColumnName("UsuarioId");

            builder.Property(e => e.ClaimType)
                .HasColumnName("Tipo");

            builder.Property(e => e.ClaimValue)
                .HasColumnName("Valor");
        }
    }
}
