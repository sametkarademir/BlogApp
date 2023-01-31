using Business.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities.Abstract;
using WebUI.Models;

namespace WebUI.Controllers.UI
{
    public class HomeController : Controller
    {
        private readonly IWebInfoService _webInfoService;

        public HomeController(IWebInfoService webInfoService)
        {
            _webInfoService = webInfoService;

        }

        public async Task<IActionResult> Index()
        {
            var webInfo = await _webInfoService.GetAsync("WebInfo");
            var data = new HomeDto();
            data.WebInfo = webInfo.Data;

            return View(data);
        }

    }
}