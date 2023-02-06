using Data.Repositories.Interface;
using Entities.Concrete.Blog;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete;

namespace Data.Repositories.Concrete;

public class ArticleRepository : EntityRepository<Article>, IArticleRepository
{
    public ArticleRepository(DbContext context) : base(context)
    {
    }
}