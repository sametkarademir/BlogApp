using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class WebInfoAddDto
{
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    public string? ImageUrl { get; set; }
    [DisplayName("İçerik")] public string? Content { get; set; }
    [Required][MaxLength(250)][DisplayName("Ünvan")] public string Title { get; set; } = null!;
    [Required][MaxLength(20)][DisplayName("Ad")] public string Name { get; set; } = null!;
    [Required][MaxLength(20)][DisplayName("Soyad")] public string Surname { get; set; } = null!;
    [Required][MaxLength(250)][DisplayName("E-Posta")] public string Email { get; set; } = null!;
    [MaxLength(20)][DisplayName("Doğum Tarihi")] public string? Birthday { get; set; }
    [MaxLength(20)][DisplayName("Şehir")] public string? City { get; set; }
    [DisplayName("Başarı Sayısı")] public int? ExperienceCount { get; set; }
    [DisplayName("Müşteri Sayısı")] public int? CustomerCount { get; set; }
    [DisplayName("Proje Sayısı")] public int? ProjectCount { get; set; }
    [DisplayName("Ödül Sayısı")] public int? AwardsCount { get; set; }
    [MaxLength(150)][DisplayName("Twitter")] public string? Twitter { get; set; }
    [MaxLength(150)][DisplayName("Facebook")] public string? Facebook { get; set; }
    [MaxLength(150)][DisplayName("Instagram")] public string? Instagram { get; set; }
    [MaxLength(150)][DisplayName("Github")] public string? Github { get; set; }
    [MaxLength(150)][DisplayName("Linkedin")] public string? Linkedin { get; set; }
    [MaxLength(150)][DisplayName("Medium")] public string? Medium { get; set; }
}

public class WebInfoUpdateDto
{
    public string? Id { get; set; }
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    public string? ImageUrl { get; set; }
    [DisplayName("İçerik")] public string? Content { get; set; }
    [Required][MaxLength(250)][DisplayName("Ünvan")] public string Title { get; set; } = null!;
    [Required][MaxLength(20)][DisplayName("Ad")] public string Name { get; set; } = null!;
    [Required][MaxLength(20)][DisplayName("Soyad")] public string Surname { get; set; } = null!;
    [Required][MaxLength(250)][DisplayName("E-Posta")] public string Email { get; set; } = null!;
    [MaxLength(20)][DisplayName("Doğum Tarihi")] public string? Birthday { get; set; }
    [MaxLength(20)][DisplayName("Şehir")] public string? City { get; set; }
    [DisplayName("Başarı Sayısı")] public int? ExperienceCount { get; set; }
    [DisplayName("Müşteri Sayısı")] public int? CustomerCount { get; set; }
    [DisplayName("Proje Sayısı")] public int? ProjectCount { get; set; }
    [DisplayName("Ödül Sayısı")] public int? AwardsCount { get; set; }
    [MaxLength(150)][DisplayName("Twitter")] public string? Twitter { get; set; }
    [MaxLength(150)][DisplayName("Facebook")] public string? Facebook { get; set; }
    [MaxLength(150)][DisplayName("Instagram")] public string? Instagram { get; set; }
    [MaxLength(150)][DisplayName("Github")] public string? Github { get; set; }
    [MaxLength(150)][DisplayName("Linkedin")] public string? Linkedin { get; set; }
    [MaxLength(150)][DisplayName("Medium")] public string? Medium { get; set; }
}