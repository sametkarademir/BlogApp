using Entities.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary key
        builder.HasKey(u => u.Id);

        // Indexes for "normalized" username and email, to allow efficient lookups
        builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

        // Maps to the AspNetUsers table
        builder.ToTable("AspNetUsers");

        // A concurrency token for use with the optimistic concurrency checking
        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        // Limit the size of columns to use efficient database types
        builder.Property(u => u.UserName).HasMaxLength(50);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(50);
        builder.Property(u => u.Email).HasMaxLength(100);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(100);

        // The relationships between User and other entity types
        // Note that these relationships are configured with no navigation properties

        // Each User can have many UserClaims
        builder.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

        // Each User can have many UserLogins
        builder.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

        // Each User can have many UserTokens
        builder.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

        builder.Property(item => item.ImageUrl).IsRequired(false);
        builder.Property(item => item.ImageUrl).HasMaxLength(250);
        builder.Property(item => item.Name).IsRequired(false);
        builder.Property(item => item.Name).HasMaxLength(20);

        builder.Property(item => item.Surname).IsRequired(false);
        builder.Property(item => item.Surname).HasMaxLength(20);

        builder.Property(item => item.CreatedBy).IsRequired(false);
        builder.Property(item => item.CreatedBy).HasMaxLength(70);

        builder.Property(item => item.ModifiedBy).IsRequired(false);
        builder.Property(item => item.ModifiedBy).HasMaxLength(70);

        builder.Property(item => item.CreatedAt).IsRequired();
        builder.Property(item => item.ModifiedAt).IsRequired();

        builder.HasData(new User
        {
            Id = 1,
            Name = "Admin",
            Surname = "Admin",
            AccessFailedCount = 0,
            Email = "admin@mail.com",
            EmailConfirmed = true,
            ImageUrl = "Storage/Images/Users/default.png",
            LockoutEnabled = true,
            PhoneNumber = "000000000",
            UserName = "Admin",
            SecurityStamp = "R4NTFFYXD5G2VQ2PYOCGVOZUNAXD2E4V",
            NormalizedEmail = "ADMIN@MAIL.COM",
            NormalizedUserName = "ADMIN",
            PasswordHash = "AQAAAAEAACcQAAAAEOpw9tRiZfEUgn/qmUnqBg1nFAvBrzWtIphs/uGCgkeA5OyFqxR9gzliKGBU8RdCiA=="
        });
    }
}
public class RoleClaimMap : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        // Primary key
        builder.HasKey(rc => rc.Id);

        // Maps to the AspNetRoleClaims table
        builder.ToTable("AspNetRoleClaims");
    }
}
public class RoleMap : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Primary key
        builder.HasKey(r => r.Id);

        // Index for "normalized" role name to allow efficient lookups
        builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

        // Maps to the AspNetRoles table
        builder.ToTable("AspNetRoles");

        // A concurrency token for use with the optimistic concurrency checking
        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

        // Limit the size of columns to use efficient database types
        builder.Property(u => u.Name).HasMaxLength(100);
        builder.Property(u => u.NormalizedName).HasMaxLength(100);

        // The relationships between Role and other entity types
        // Note that these relationships are configured with no navigation properties

        // Each Role can have many entries in the UserRole join table
        builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

        // Each Role can have many associated RoleClaims
        builder.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

        builder.HasData(new Role
        {
            Id = 1,
            Name = "Admin",
            NormalizedName = "ADMIN"

        });
    }
}
public class UserClaimMap : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        // Primary key
        builder.HasKey(uc => uc.Id);

        // Maps to the AspNetUserClaims table
        builder.ToTable("AspNetUserClaims");
    }
}
public class UserLoginMap : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        // Composite primary key consisting of the LoginProvider and the key to use
        // with that provider
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

        // Limit the size of the composite key columns due to common DB restrictions
        builder.Property(l => l.LoginProvider).HasMaxLength(128);
        builder.Property(l => l.ProviderKey).HasMaxLength(128);

        // Maps to the AspNetUserLogins table
        builder.ToTable("AspNetUserLogins");
    }
}
public class UserRoleMap : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Primary key
        builder.HasKey(r => new { r.UserId, r.RoleId });

        // Maps to the AspNetUserRoles table
        builder.ToTable("AspNetUserRoles");

        builder.HasData(new UserRole
        {
            RoleId = 1,
            UserId = 1
        });
    }
}
public class UserTokenMap : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        // Composite primary key consisting of the UserId, LoginProvider and Name
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        // Limit the size of the composite key columns due to common DB restrictions
        builder.Property(t => t.LoginProvider).HasMaxLength(256);
        builder.Property(t => t.Name).HasMaxLength(256);

        // Maps to the AspNetUserTokens table
        builder.ToTable("AspNetUserTokens");
    }
}