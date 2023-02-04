using Data.Repositories.Interface;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete;

namespace Data.Repositories.Concrete;

public class SystemLogRepository : EntityRepository<SystemLog>, ISystemLogRepository
{
    public SystemLogRepository(DbContext context) : base(context)
    {
    }
}