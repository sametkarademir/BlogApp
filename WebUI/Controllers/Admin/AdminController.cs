using AutoMapper;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers.Admin
{
    [Authorize(Roles = "Admin,User")]
    public class AdminController : BaseController
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(string message)
        {
            return View();
        }


        public AdminController(UserManager<User> userManager, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor) : base(userManager, mapper, toastNotification, httpContextHelper, httpContextAccessor)
        {
        }
    }
}
