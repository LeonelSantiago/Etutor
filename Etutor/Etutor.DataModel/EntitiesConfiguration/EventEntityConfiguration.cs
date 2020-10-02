using Etutor.DataModel.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Etutor.DataModel.EntitiesConfiguration
{
    public class EventEntityConfiguration : IEntityTypeConfiguration<Events>
    {
        private readonly Context.ApplicationDbContext _context;

        public void Configure(EntityTypeBuilder<Events> builder)
        {
            builder.ToTable("Events");

            builder.Property(e => e.Id)
                .HasColumnName("Id");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.StartDate)
                .IsRequired();

            builder.Property(e => e.EndDate);

            builder.Property(e => e.StartTime)
                .IsRequired();


            builder.Property(e => e.EventTypeId)
                .IsRequired();

            builder.HasOne<EventsTypes>();

        }
    
    }
}
