using Data.Repositories.Interface;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete;

namespace Data.Repositories.Concrete;

public class ResumeRepository : EntityRepository<Resume>, IResumeRepository
{
    public ResumeRepository(DbContext context) : base(context)
    {
    }
}