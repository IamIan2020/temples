using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Temples.Core.Entities;

namespace Temples.Infrastructure.Configurations;

public class ServiceItemOptionConfiguration : IEntityTypeConfiguration<ServiceItemOption>
{
    public void Configure(EntityTypeBuilder<ServiceItemOption> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Title).IsRequired().HasMaxLength(200);
        builder.Property(o => o.PriceUnit).HasMaxLength(50);
        builder.Property(o => o.SubTitle).HasMaxLength(200);
        builder.Property(o => o.Description).HasMaxLength(1000);
        builder.Property(o => o.Price).HasColumnType("decimal(18,2)");
        builder.Property(o => o.MaxTotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.CurrentTotalAmount).HasColumnType("decimal(18,2)");
    }
}
