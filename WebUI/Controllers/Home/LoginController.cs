using AutoMapper;
using Business.Dtos;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Shared.Entities.Abstract;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers.Home
{
    public class LoginController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        public LoginController(UserManager<User> userManager, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(userManager, mapper, toastNotification, httpContextHelper, httpContextAccessor)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
        [HttpGet]
        public IActionResult Error() => View();
        [HttpGet]
        public IActionResult Index() => View();
        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            var serviceInputDto = await HttpContextHelper.GetHttpContextContexObject(HttpContextAccessor);
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(loginDto.Username);
                    if (user != null)
                    {

                        if (await UserManager.IsLockedOutAsync(user))
                        {
                            ToastNotification.AddInfoToastMessage("Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                            return View();
                        }
                        await _signInManager.SignOutAsync();

                        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, true, true);
                        if (result.Succeeded)
                        {
                            await UserManager.ResetAccessFailedCountAsync(user);


                            ToastNotification.AddSuccessToastMessage("Hoş Geldiniz.");
                            return RedirectToAction("Index", "Admin");
                        }
                        await UserManager.AccessFailedAsync(user);
                        var fail = await UserManager.GetAccessFailedCountAsync(user);
                        if (fail > 2)
                        {
                            await UserManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ToastNotification.AddWarningToastMessage("Hesabınız çok fazla başarısız girişten dolayı kitlenmiştir.");
                            return View();
                        }



                        ToastNotification.AddInfoToastMessage("Kullanıcı adı veya parola hatalı.");
                        return View(loginDto);

                    }
                    else
                    {


                        ToastNotification.AddInfoToastMessage("Kullanıcı adı veya parola hatalı.");
                        return View(loginDto);
                    }
                }
                ToastNotification.AddInfoToastMessage("Eksik veya hatalı giriş yaptınız. Lütfen tekrar deneyiniz.");
                return View(loginDto);

            }
            catch
            {
                ToastNotification.AddInfoToastMessage("Beklenmedik bir hata oluştu. Lütefen tekrar deneyiniz.");
                return RedirectToAction("Error", "Login");
            }
        }
        public async Task<IActionResult> Logout()
        {
            var httpContextObject = await HttpContextHelper.GetHttpContextContexObject(HttpContextAccessor);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Login");
        }

    }
}
