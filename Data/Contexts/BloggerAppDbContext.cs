using Data.Mappings;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class BloggerAppDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public DbSet<WebInfo>? WebInfos { get; set; }

    public BloggerAppDbContext(DbContextOptions<BloggerAppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new RoleClaimMap());
        modelBuilder.ApplyConfiguration(new UserClaimMap());
        modelBuilder.ApplyConfiguration(new UserLoginMap());
        modelBuilder.ApplyConfiguration(new UserRoleMap());
        modelBuilder.ApplyConfiguration(new UserTokenMap());

        modelBuilder.ApplyConfiguration(new WebInfoMap());

    }
}