using AutoMapper;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers;

public class BaseController : Controller
{
    protected UserManager<User> UserManager { get; }
    protected IMapper Mapper { get; }
    protected IToastNotification ToastNotification;
    protected IHttpContextHelper HttpContextHelper { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }

    public BaseController(UserManager<User> userManager, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor)
    {
        UserManager = userManager;
        Mapper = mapper;
        ToastNotification = toastNotification;
        HttpContextHelper = httpContextHelper;
        HttpContextAccessor = httpContextAccessor;
    }
}