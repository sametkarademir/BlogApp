using Data.Repositories.Interface;
using Entities.Concrete.Blog;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete;

namespace Data.Repositories.Concrete;

public class CategoryRepository : EntityRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DbContext context) : base(context)
    {
    }
}