using Business.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.WebSetting;

[Authorize(Roles = "Admin,Home")]
public class WebController : Controller
{
    private readonly ISystemLogService _systemLogService;

    public WebController(ISystemLogService systemLogService)
    {
        _systemLogService = systemLogService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _systemLogService.GetAllRequestCountAsync();
        return View(result.Data);
    }
}