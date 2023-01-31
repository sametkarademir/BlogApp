using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class WebInfoMap : IEntityTypeConfiguration<WebInfo>
{
    public void Configure(EntityTypeBuilder<WebInfo> builder)
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

        builder.Property(item => item.ImageUrl).IsRequired(false);
        builder.Property(item => item.ImageUrl).HasMaxLength(250);

        builder.Property(item => item.Content).IsRequired(false);

        builder.Property(item => item.Title).IsRequired(false);
        builder.Property(item => item.Title).HasMaxLength(250);

        builder.Property(item => item.Name).IsRequired(false);
        builder.Property(item => item.Name).HasMaxLength(20);

        builder.Property(item => item.Surname).IsRequired(false);
        builder.Property(item => item.Surname).HasMaxLength(20);

        builder.Property(item => item.Email).IsRequired(false);
        builder.Property(item => item.Email).HasMaxLength(250);

        builder.Property(item => item.Birthday).IsRequired(false);
        builder.Property(item => item.Birthday).HasMaxLength(20);

        builder.Property(item => item.City).IsRequired(false);
        builder.Property(item => item.City).HasMaxLength(250);

        builder.Property(item => item.ExperienceCount).IsRequired(false);
        builder.Property(item => item.CustomerCount).IsRequired(false);
        builder.Property(item => item.ProjectCount).IsRequired(false);
        builder.Property(item => item.AwardsCount).IsRequired(false);
        
        builder.Property(item => item.Twitter).IsRequired(false);
        builder.Property(item => item.Twitter).HasMaxLength(250);

        builder.Property(item => item.Facebook).IsRequired(false);
        builder.Property(item => item.Facebook).HasMaxLength(250);

        builder.Property(item => item.Instagram).IsRequired(false);
        builder.Property(item => item.Instagram).HasMaxLength(250);

        builder.Property(item => item.Github).IsRequired(false);
        builder.Property(item => item.Github).HasMaxLength(250);

        builder.Property(item => item.Linkedin).IsRequired(false);
        builder.Property(item => item.Linkedin).HasMaxLength(250);

        builder.Property(item => item.Medium).IsRequired(false);
        builder.Property(item => item.Medium).HasMaxLength(250);

        builder.HasData(new WebInfo
        {
            Id = "WebInfo",
            Title = "Başlık",
            Name = "Samet",
            Surname = "Karademir",
            City = "İstanbul",
            AwardsCount = 0,
            Birthday = "1995",
            Content = "",
            CreatedAt = 0,
            CreatedBy = "System",
            CustomerCount = 0,
            Email = "sametkarademir244@gmail.com",
            ExperienceCount = 0,
            Facebook = "",
            Github = "",
            ImageUrl = "",
            Instagram = "",
            Linkedin = "",
            Medium = "",
            ModifiedAt = 0,
            ModifiedBy = "System",
            ProjectCount = 0,
            Status = 0,
            Twitter = ""
        });
    }
}