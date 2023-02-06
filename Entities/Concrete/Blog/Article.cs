using Shared.Entities.Abstract;

namespace Entities.Concrete.Blog;

public class Article : BaseEntity
{
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public string? ContentUrl { get; set; }
    public string? SeoAuthor { get; set; }
    public string? SeoDescription { get; set; }
    public string? SeoTag { get; set; }
    public string? CategoryId { get; set; }
    public Category? Category { get; set; }
}