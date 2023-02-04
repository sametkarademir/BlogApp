using Data.Repositories.Interface;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete;

namespace Data.Repositories.Concrete;

public class FolderRepository : EntityRepository<Folder>, IFolderRepository
{
    public FolderRepository(DbContext context) : base(context)
    {
    }
}