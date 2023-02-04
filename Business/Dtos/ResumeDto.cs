using Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Dtos;

public class ResumeDto
{
    public Resume? Resume { get; set; }
}

public class ResumeListDto
{
    public List<Resume> Resumes { get; set; }
}

public class ResumeAddDto
{
    [Required][MaxLength(250)][DisplayName("Ünvan")] public string Title { get; set; } = null!;
    [MaxLength(250)][DisplayName("Şirket")] public string? Company { get; set; }
    [MaxLength(250)][DisplayName("Çalaşıma Süresi")] public string? Date { get; set; }
    [MaxLength(1000)][DisplayName("Açıklama")] public string? Description { get; set; }
}

public class ResumeUpdateDto
{
    public string? Id { get; set; }
    [Required][MaxLength(250)][DisplayName("Ünvan")] public string Title { get; set; } = null!;
    [MaxLength(250)][DisplayName("Şirket")] public string? Company { get; set; }
    [MaxLength(250)][DisplayName("Çalaşıma Süresi")] public string? Date { get; set; }
    [MaxLength(1000)][DisplayName("Açıklama")] public string? Description { get; set; }
}