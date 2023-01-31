using Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Dtos;

public class RoleDto
{
    public Role? Role { get; set; }
}
public class RoleListDto
{
    public List<Role> Roles { get; set; } = null!;
}
public class RoleAddDto
{
    [Required][MaxLength(20)][DisplayName("Yetki")] public string Name { get; set; } = null!;
    [MaxLength(300)][DisplayName("Açıklama")] public string? Description { get; set; }
}