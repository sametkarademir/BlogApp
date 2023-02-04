using Shared.Entities.Abstract;

namespace Entities.Concrete;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public string? ContentUrl { get; set; }
    public string? DemoUrl { get; set; }
}