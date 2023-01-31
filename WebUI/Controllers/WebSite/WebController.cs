using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.WebSite
{
    public class WebController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
