using Shared.Entities.Abstract;

namespace Entities.Concrete;

public class Resume : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Company { get; set; }
    public string? Date { get; set; }
    public string? Description { get; set; }
}