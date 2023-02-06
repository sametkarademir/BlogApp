using Entities.Concrete.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class ArticleMap : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
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

        builder.Property(item => item.ImageUrl).IsRequired(false);
        builder.Property(item => item.ImageUrl).HasMaxLength(250);

        builder.Property(item => item.ContentUrl).IsRequired(false);
        builder.Property(item => item.ContentUrl).HasMaxLength(250);

        builder.Property(item => item.SeoAuthor).IsRequired(false);
        builder.Property(item => item.SeoAuthor).HasMaxLength(50);

        builder.Property(item => item.SeoDescription).IsRequired(false);
        builder.Property(item => item.SeoDescription).HasMaxLength(500);

        builder.Property(item => item.SeoTag).IsRequired(false);
        builder.Property(item => item.SeoTag).HasMaxLength(70);

        builder.HasOne<Category>(item => item.Category).WithMany(x=> x.Articles).HasForeignKey(x => x.CategoryId);
    }
}