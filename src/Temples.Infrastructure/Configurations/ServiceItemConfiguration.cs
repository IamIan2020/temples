using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Temples.Core.Entities;

namespace Temples.Infrastructure.Configurations;

public class ServiceItemConfiguration : IEntityTypeConfiguration<ServiceItem>
{
    public void Configure(EntityTypeBuilder<ServiceItem> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).IsRequired().HasMaxLength(200);
        builder.Property(s => s.HeaderImage).HasMaxLength(500);
        builder.HasMany(s => s.Options)
            .WithOne(o => o.ServiceItem)
            .HasForeignKey(o => o.ServiceItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
