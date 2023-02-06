using Entities.Concrete.Blog;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ArticleDto
{
    public Article? Article { get; set; }
}

public class ArticleListDto : DtoGetBase
{
    public List<Article>? Articles { get; set; }
    public string? CategoryId { get; set; }
}


public class ArticleAddDto
{
    [Required][MaxLength(250)][DisplayName("Başlık")] public string Title { get; set; } = null!;
    public string? ImageUrl { get; set; }
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    public string? ContentUrl { get; set; }
    [DisplayName("İçerik")] public string? Content { get; set; }
    [Required][MaxLength(50)][DisplayName("Yazar")] public string SeoAuthor { get; set; } = null!;
    [Required][MaxLength(500)][DisplayName("Açıklama")] public string SeoDescription { get; set; } = null!;
    [Required][MaxLength(70)][DisplayName("Etiket")] public string SeoTag { get; set; } = null!;
    [Required][DisplayName("Kategori")] public string CategoryId { get; set; } = null!;
    public List<Category>? Categories { get; set; }
}

public class ArticleUpdateDto
{
    public string? Id { get; set; }
    [Required][MaxLength(250)][DisplayName("Başlık")] public string Title { get; set; } = null!;
    public string? ImageUrl { get; set; }
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    public string? ContentUrl { get; set; }
    [DisplayName("İçerik")] public string? Content { get; set; }
    [Required][MaxLength(50)][DisplayName("Yazar")] public string SeoAuthor { get; set; } = null!;
    [Required][MaxLength(500)][DisplayName("Açıklama")] public string SeoDescription { get; set; } = null!;
    [Required][MaxLength(70)][DisplayName("Etiket")] public string SeoTag { get; set; } = null!;
    [Required][DisplayName("Kategori")] public string CategoryId { get; set; } = null!;
    public List<Category>? Categories { get; set; }
}