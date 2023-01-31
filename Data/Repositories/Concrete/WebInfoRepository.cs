using Data.Repositories.Interface;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete;

namespace Data.Repositories.Concrete;

public class WebInfoRepository : EntityRepository<WebInfo>, IWebInfoRepository
{
    public WebInfoRepository(DbContext context) : base(context)
    {
    }
}