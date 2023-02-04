using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class ProjectMap : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
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

        builder.Property(item => item.Name).IsRequired(false);
        builder.Property(item => item.Name).HasMaxLength(250);

        builder.Property(item => item.Category).IsRequired(false);
        builder.Property(item => item.Category).HasMaxLength(50);

        builder.Property(item => item.ImageUrl).IsRequired(false);
        builder.Property(item => item.ImageUrl).HasMaxLength(250);

        builder.Property(item => item.ContentUrl).IsRequired(false);
        builder.Property(item => item.ContentUrl).HasMaxLength(250);

        builder.Property(item => item.DemoUrl).IsRequired(false);
        builder.Property(item => item.DemoUrl).HasMaxLength(250);
    }
}