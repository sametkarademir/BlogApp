using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class SystemLogMap : IEntityTypeConfiguration<SystemLog>
{
    public void Configure(EntityTypeBuilder<SystemLog> builder)
    {
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedOnAdd();

        builder.Property(item => item.Message).IsRequired(false);

        builder.Property(item => item.Username).IsRequired(false);
        builder.Property(item => item.Username).HasMaxLength(30);

        builder.Property(item => item.Date).IsRequired();

        builder.Property(item => item.RemoteAddress).IsRequired(false);
        builder.Property(item => item.RemoteAddress).HasMaxLength(20);

        builder.Property(item => item.RemotePort).IsRequired(false);
        builder.Property(item => item.RemotePort).HasMaxLength(10);

        builder.Property(item => item.Action).IsRequired(false);
        builder.Property(item => item.Action).HasMaxLength(30);

        builder.Property(item => item.Method).IsRequired(false);
        builder.Property(item => item.Method).HasMaxLength(30);

        builder.Property(item => item.LogStatus).IsRequired();
    }
}