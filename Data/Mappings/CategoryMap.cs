using Entities.Concrete.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
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
        builder.Property(item => item.Name).HasMaxLength(50);

        builder.Property(item => item.Color).IsRequired(false);
        builder.Property(item => item.Color).HasMaxLength(10);
    }
}