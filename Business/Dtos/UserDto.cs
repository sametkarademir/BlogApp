using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Dtos; 
public class UserDto { }

public class UserListDto
{
    public IList<User> Users { get; set; } = null!;
}
public class UserAddDto
{
    [Required][MaxLength(20)][DisplayName("Ad")] public string Name { get; set; } = null!;
    [Required][MaxLength(20)][DisplayName("Soyad")] public string Surname { get; set; } = null!;
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    public string? ImageUrl { get; set; }
    [MaxLength(20)] public string UserName { get; set; } = null!;
    [MaxLength(16)] public string Password { get; set; } = null!;
    [MaxLength(150)][EmailAddress] public string Email { get; set; } = null!;
    [MaxLength(11)] public string? PhoneNumber { get; set; }
}
public class UserUpdateDto
{
    [Required][MaxLength(20)][DisplayName("Ad")] public string Name { get; set; } = null!;
    [Required][MaxLength(20)][DisplayName("Soyad")] public string Surname { get; set; } = null!;
    [DisplayName("Resim")] public IFormFile? File { get; set; }
    public string? ImageUrl { get; set; }
    [MaxLength(150)] public string UserName { get; set; } = null!;
    [MaxLength(150)] public string? Password { get; set; }
    [MaxLength(150)] public string Email { get; set; } = null!;
    [MaxLength(150)] public string? PhoneNumber { get; set; }
    [Required] public int Id { get; set; }
}

public class LoginDto
{
    [Display(Name = "Kullanıcı Adı")][Required(ErrorMessage = "Boş Bırakılamaz")] public string Username { get; set; } = null!;
    [Display(Name = "Parola")][Required(ErrorMessage = "Boş Bırakılamaz")] public string Password { get; set; } = null!;
}

public class AuthorWithRolesViewModel
{
    public User? User { get; set; }
    public IList<string>? Roles { get; set; }
}
public class UserRoleAssignDto
{
    public UserRoleAssignDto()
    {
        RoleAssignDtos = new List<RoleAssignDto>();
    }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public IList<RoleAssignDto>? RoleAssignDtos { get; set; }
}
public class RoleAssignDto
{
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public bool HasRole { get; set; } = false;
}
