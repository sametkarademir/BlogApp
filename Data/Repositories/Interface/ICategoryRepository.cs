using Entities.Concrete.Blog;
using Shared.Data.Interface;

namespace Data.Repositories.Interface;

public interface ICategoryRepository : IEntityRepository<Category>
{
}