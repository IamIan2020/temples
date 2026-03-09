using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Temples.Core.Entities;

namespace Temples.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.ChineseName).HasMaxLength(50);
        builder.Property(u => u.Gender).HasMaxLength(10);
        builder.Property(u => u.Address).HasMaxLength(500);
        builder.Property(u => u.MemberNumber).HasMaxLength(50);
    }
}
