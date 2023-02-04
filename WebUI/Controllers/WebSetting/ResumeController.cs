using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers.WebSetting;

[Authorize(Roles = "Admin,Home")]
public class ResumeController : Controller
{
    private readonly IResumeService _resumeService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toastNotification;
    private readonly IHttpContextHelper _httpContextHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResumeController(IResumeService resumeService, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor)
    {
        _resumeService = resumeService;
        _mapper = mapper;
        _toastNotification = toastNotification;
        _httpContextHelper = httpContextHelper;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _resumeService.GetAllAsync(null, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            return View(result.Data);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResumeAddDto addDto)
    {
        if (ModelState.IsValid)
        {
            #region Get HttpContext
            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            #endregion
            var result = await _resumeService.CreateAsync(addDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Kaydedildi");
                return RedirectToAction("Index", "Resume");
            }
        }
        else
        {
            _toastNotification.AddInfoToastMessage("Alanları istendiği gibi doldurunuz.");
            return View(addDto);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    [HttpGet]
    public async Task<IActionResult> Update(string Id)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _resumeService.GetAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            var data = _mapper.Map<ResumeUpdateDto>(result.Data.Resume);
            return View(data);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> Update(ResumeUpdateDto updateDto)
    {
        if (ModelState.IsValid)
        {
            #region Get HttpContext
            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            #endregion
            var result = await _resumeService.UpdateAsync(updateDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Güncellendi");
                return RedirectToAction("Index", "Resume");
            }
        }
        else
        {
            _toastNotification.AddInfoToastMessage("Alanları istendiği gibi doldurunuz.");
            return View(updateDto);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    public async Task<IActionResult> Confirmation(string Id)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _resumeService.ConfirmationAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Durumu Değiştirildi");
            return RedirectToAction("Index", "Resume");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    public async Task<IActionResult> Delete(string Id)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _resumeService.DeleteAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Silindi");
            return RedirectToAction("Index", "Resume");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }
}