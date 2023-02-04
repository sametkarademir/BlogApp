using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class ResumeMap : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedOnAdd();

        builder.Property(item => item.CreatedBy).IsRequired(false);
        builder.Property(item => item.CreatedBy).HasMaxLength(70);

        builder.Property(item => item.ModifiedBy).IsRequired(false);
        builder.Property(item => item.ModifiedBy).HasMaxLength(70);

        builder.Property(item => item.Status).IsRequired();

        builder.Property(item => item.CreatedAt).IsRequired();
        builder.Property(item => item.ModifiedAt).IsRequired();

        builder.Property(item => item.Title).IsRequired(false);
        builder.Property(item => item.Title).HasMaxLength(250);

        builder.Property(item => item.Company).IsRequired(false);
        builder.Property(item => item.Company).HasMaxLength(250);

        builder.Property(item => item.Date).IsRequired(false);
        builder.Property(item => item.Date).HasMaxLength(250);

        builder.Property(item => item.Description).IsRequired(false);
        builder.Property(item => item.Description).HasMaxLength(1000);
    }
}