using Business.Dtos;
using Entities.Concrete;

namespace WebUI.Models;

public class HomeDto
{
    public WebInfo? WebInfo { get; set; }
    public ResumeListDto? ResumeListDto { get; set; }
    public ProjectListDto? ProjectListDto { get; set; }
}
