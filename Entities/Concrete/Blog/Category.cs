using Shared.Entities.Abstract;

namespace Entities.Concrete.Blog;

public class Category : BaseEntity
{
    public string? Name { get; set; }
    public string? Color { get; set; }
    public ICollection<Article>? Articles { get; set; }
}