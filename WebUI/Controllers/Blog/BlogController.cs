using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Blog;

[Authorize(Roles = "Admin,Blog")]
public class BlogController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}