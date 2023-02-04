using Shared.Entities.Abstract;

namespace Entities.Concrete;

public class Folder : BaseEntity
{
    public string Name { get; set; } = null!;
    public string ObjectId { get; set; } = null!;
    public string ObjectName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string? Size { get; set; }
    public string? OldName { get; set; }
    public string? Extension { get; set; }
}