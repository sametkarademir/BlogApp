using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Shared.Entities.Abstract;
using WebUI.Helpers.Interface;
using WebUI.Models;

namespace WebUI.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IWebInfoService _webInfoService;
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IToastNotification _toastNotification;
        private readonly ISystemLogService _systemLogService;
        private readonly IResumeService _resumeService;
        private readonly IProjectService _projectService;

        public HomeController(IWebInfoService webInfoService, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor, IToastNotification toastNotification, ISystemLogService systemLogService, IResumeService resumeService, IProjectService projectService)
        {
            _webInfoService = webInfoService;
            _httpContextHelper = httpContextHelper;
            _httpContextAccessor = httpContextAccessor;
            _toastNotification = toastNotification;
            _systemLogService = systemLogService;
            _resumeService = resumeService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            try
            {
                var webInfo = await _webInfoService.GetAsync("WebInfo", serviceInputDto);
                var resumeList = await _resumeService.GetAllAsync(Status.Active, serviceInputDto);
                var projectList = await _projectService.GetAllAsync(Status.Active, serviceInputDto);

                resumeList.Data.Resumes = resumeList.Data.Resumes.OrderByDescending(item => item.CreatedAt).ToList();
                projectList.Data.Projects = projectList.Data.Projects?.OrderByDescending(item => item.CreatedAt).ToList();
                var data = new HomeDto
                {
                    WebInfo = webInfo.Data,
                    ResumeListDto = resumeList.Data,
                    ProjectListDto = projectList.Data
                };
                return View(data);
            }
            catch (Exception e)
            {
                await _systemLogService.CreateAsync(new SystemLogAddDto
                {

                    Date = DateTime.UtcNow,
                    LogStatus = LogStatus.Error,
                    Message = $"Message : {e.Message} - Detail : {e.StackTrace}",
                    Method = serviceInputDto.RemoteAction,
                    Action = serviceInputDto.RemoteController,
                    RemoteAddress = serviceInputDto.RemoteAddress,
                    RemotePort = serviceInputDto.RemotePort,
                    Username = serviceInputDto.Username
                });
            }
            _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
            return RedirectToAction("Error", "Home");

        }
    }
}