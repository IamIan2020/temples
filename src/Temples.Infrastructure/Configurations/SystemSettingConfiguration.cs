using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Temples.Core.Entities;

namespace Temples.Infrastructure.Configurations;

public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.CompanyName).IsRequired().HasMaxLength(200);
        builder.Property(s => s.WebsiteName).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Phone).HasMaxLength(50);
        builder.Property(s => s.TaxId).HasMaxLength(20);
        builder.Property(s => s.Copyright).IsRequired().HasMaxLength(500);
    }
}
