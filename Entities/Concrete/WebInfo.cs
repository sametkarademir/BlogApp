using Shared.Entities.Abstract;

namespace Entities.Concrete;

public class WebInfo : BaseEntity
{
    public string? ImageUrl { get; set; }
    public string? Content { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Birthday { get; set; }
    public string? City { get; set; }
    public int? ExperienceCount { get; set; }
    public int? CustomerCount { get; set; }
    public int? ProjectCount { get; set; }
    public int? AwardsCount { get; set; }
    public string? Twitter { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Github { get; set; }
    public string? Linkedin { get; set; }
    public string? Medium { get; set; }
}