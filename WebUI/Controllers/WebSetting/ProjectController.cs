using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Shared.Utilities.Result;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers.WebSetting;

[Authorize(Roles = "Admin,Home")]
public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toastNotification;
    private readonly IHttpContextHelper _httpContextHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IImageHelper _imageHelper;
    public ProjectController(IProjectService projectService, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper)
    {
        _projectService = projectService;
        _mapper = mapper;
        _toastNotification = toastNotification;
        _httpContextHelper = httpContextHelper;
        _httpContextAccessor = httpContextAccessor;
        _imageHelper = imageHelper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _projectService.GetAllAsync(null, serviceInputDto);
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
    public async Task<IActionResult> Create(ProjectAddDto addDto)
    {
        if (ModelState.IsValid)
        {
            #region Get HttpContext
            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            #endregion

            var contentCreateResult = await _imageHelper.CreateContentFolder(addDto.Content, addDto.Name, addDto.Name, nameof(Project), serviceInputDto);
            if (contentCreateResult?.Data != null && contentCreateResult?.ExeptionStatus == ExeptionStatus.Success)
            {
                addDto.ContentUrl = contentCreateResult.Data.FolderUrl;
                if (addDto.File != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImgFolder(addDto.File, addDto.Name, addDto.Name, nameof(Project), serviceInputDto);
                    if (imageUploadResult?.Data != null && imageUploadResult.ExeptionStatus == ExeptionStatus.Success)
                    {
                        if (addDto.ImageUrl != null) { await _imageHelper.Delete(addDto.ImageUrl, serviceInputDto); }
                        addDto.ImageUrl = imageUploadResult.Data?.FolderUrl;
                    }
                }
            }
            else
            {
                _toastNotification.AddInfoToastMessage("İçerik kaydedilemedi");
                return View(addDto);
            }
            var result = await _projectService.CreateAsync(addDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Kaydedildi");
                return RedirectToAction("Index", "Project");
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
        var result = await _projectService.GetAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            var data = _mapper.Map<ProjectUpdateDto>(result.Data.Project);
            data.Content = await _imageHelper.ShowReadContent(data.ContentUrl ?? "");
            return View(data);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProjectUpdateDto updateDto)
    {
        if (ModelState.IsValid)
        {
            #region Get HttpContext
            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            #endregion
            var contentCreateResult = await _imageHelper.CreateContentFolder(updateDto.Content ?? "", updateDto.Name, updateDto.Name, nameof(Project), serviceInputDto);
            if (contentCreateResult?.Data != null && contentCreateResult?.ExeptionStatus == ExeptionStatus.Success)
            {
                await _imageHelper.Delete(updateDto.ContentUrl ?? "", serviceInputDto);
                updateDto.ContentUrl = contentCreateResult.Data.FolderUrl;
                if (updateDto.File != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImgFolder(updateDto.File, updateDto.Name, updateDto.Name, nameof(Project), serviceInputDto);
                    if (imageUploadResult?.Data != null && imageUploadResult.ExeptionStatus == ExeptionStatus.Success)
                    {
                        if (updateDto.ImageUrl != null) { await _imageHelper.Delete(updateDto.ImageUrl, serviceInputDto); }
                        updateDto.ImageUrl = imageUploadResult.Data?.FolderUrl;
                    }
                }
            }
            var result = await _projectService.UpdateAsync(updateDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Güncellendi");
                return RedirectToAction("Index", "Project");
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
        var result = await _projectService.ConfirmationAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Durumu Değiştirildi");
            return RedirectToAction("Index", "Project");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    public async Task<IActionResult> Delete(string Id)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _projectService.DeleteAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Silindi");
            return RedirectToAction("Index", "Project");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }
}