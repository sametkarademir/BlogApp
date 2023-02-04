using Entities.Concrete;

namespace Business.Dtos;

public class FolderDto
{
    public Folder? Folder { get; set; }
}

public class FolderListDto
{
    public List<Folder>? Folders { get; set; }
}

public class FolderAddDto
{
    public string Name { get; set; } = null!;
    public string ObjectId { get; set; } = null!;
    public string ObjectName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string? Size { get; set; }
    public string? OldName { get; set; }
    public string? Extension { get; set; }
}