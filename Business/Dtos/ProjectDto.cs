using Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace Business.Dtos;

public class ProjectDto
{
    public Project? Project { get; set; }
}

public class ProjectListDto
{
    public List<Project>? Projects { get; set; }
}

public class ProjectAddDto
{
    [Required][MaxLength(250)][DisplayName("Proje Adı")] public string Name { get; set; } = null!;
    [MaxLength(250)][DisplayName("Kategori")] public string? Category { get; set; }
    [MaxLength(250)] public string? ImageUrl { get; set; }
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    [MaxLength(250)] public string? ContentUrl { get; set; }
    [DisplayName("İçerik")] public string? Content { get; set; }
    [MaxLength(250)][DisplayName("Demo Url")] public string? DemoUrl { get; set; }
}

public class ProjectUpdateDto
{
    public string? Id { get; set; }
    [Required][MaxLength(250)][DisplayName("Proje Adı")] public string Name { get; set; } = null!;
    [MaxLength(250)][DisplayName("Kategori")] public string? Category { get; set; }
    [MaxLength(250)] public string? ImageUrl { get; set; }
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    [MaxLength(250)] public string? ContentUrl { get; set; }
    [DisplayName("İçerik")] public string? Content { get; set; }
    [MaxLength(250)][DisplayName("Demo Url")] public string? DemoUrl { get; set; }
}