using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Blog.ViewComponents;

public class CategoryViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}

