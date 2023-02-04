using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class FolderMap : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
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

        builder.Property(item => item.ObjectId).IsRequired(false);
        builder.Property(item => item.ObjectId).HasMaxLength(250);

        builder.Property(item => item.ObjectName).IsRequired(false);
        builder.Property(item => item.ObjectName).HasMaxLength(30);

        builder.Property(item => item.Url).IsRequired(false);
        builder.Property(item => item.Url).HasMaxLength(250);

        builder.Property(item => item.Size).IsRequired(false);
        builder.Property(item => item.Size).HasMaxLength(20);

        builder.Property(item => item.OldName).IsRequired(false);
        builder.Property(item => item.OldName).HasMaxLength(250);

        builder.Property(item => item.Extension).IsRequired(false);
        builder.Property(item => item.Extension).HasMaxLength(20);
    }
}