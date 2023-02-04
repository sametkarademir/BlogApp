using Data.Repositories.Interface;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete;

namespace Data.Repositories.Concrete;

public class ProjectRepository : EntityRepository<Project>, IProjectRepository
{
    public ProjectRepository(DbContext context) : base(context)
    {
    }
}