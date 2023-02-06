using Entities.Concrete.Blog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Dtos;

public class CategoryDto
{
    public Category? Category { get; set; } 
}

public class CategoryListDto
{
    public List<Category>? Categories { get; set; }
}

public class CategoryAddDto
{
    [Required]
    [MaxLength(50)]
    [DisplayName("Ad")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    [DisplayName("Renk")]
    public string Color { get; set; } = null!;
}

public class CategoryUpdateDto
{
    public string? Id { get; set; }
    [Required][MaxLength(50)][DisplayName("Ad")] public string Name { get; set; } = null!;
    [Required][MaxLength(10)][DisplayName("Renk")] public string Color { get; set; } = null!;
}