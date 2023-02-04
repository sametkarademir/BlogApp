using Data.Mappings;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class BloggerAppDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public DbSet<WebInfo>? WebInfos { get; set; }
    public DbSet<SystemLog>? SystemLogs { get; set; }
    public DbSet<Folder>? Folders { get; set; }
    public DbSet<Resume>? Resumes { get; set; }
    public DbSet<Project>? Projects { get; set; }

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
        modelBuilder.ApplyConfiguration(new SystemLogMap());
        modelBuilder.ApplyConfiguration(new FolderMap());
        modelBuilder.ApplyConfiguration(new ResumeMap());
        modelBuilder.ApplyConfiguration(new ProjectMap());

    }
}