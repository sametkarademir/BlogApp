using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Entities.Concrete.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;
using WebUI.Helpers.Concrete;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers.Blog;

[Authorize(Roles = "Admin,Blog")]
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;
    private readonly ICategoryService _categoryService;
    private readonly IImageHelper _imageHelper;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toastNotification;
    private readonly IHttpContextHelper _httpContextHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ArticleController(IArticleService articleService, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper, ICategoryService categoryService)
    {
        _articleService = articleService;
        _mapper = mapper;
        _toastNotification = toastNotification;
        _httpContextHelper = httpContextHelper;
        _httpContextAccessor = httpContextAccessor;
        _imageHelper = imageHelper;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _articleService.GetAllAsync(null, serviceInputDto, true);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            return View(result.Data);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion

        var result = await _categoryService.GetAllAsync(Status.Active, serviceInputDto);
        var addDto = new ArticleAddDto { Categories = result.Data.Categories };
        return View(addDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ArticleAddDto addDto)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        if (ModelState.IsValid)
        {
            var contentCreateResult = await _imageHelper.CreateContentFolder(addDto.Content, addDto.Title, addDto.Title, nameof(Article), serviceInputDto);
            if (contentCreateResult?.Data != null && contentCreateResult?.ExeptionStatus == ExeptionStatus.Success)
            {
                addDto.ContentUrl = contentCreateResult.Data.FolderUrl;
                if (addDto.File != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImgFolder(addDto.File, addDto.Title, addDto.Title, nameof(Article), serviceInputDto);
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
            var result = await _articleService.CreateAsync(addDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Kaydedildi");
                return RedirectToAction("Index", "Article");
            }
        }
        else
        {
            var categoryResult = await _categoryService.GetAllAsync(Status.Active, serviceInputDto);
            addDto.Categories = categoryResult.Data.Categories;
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
        var result = await _articleService.GetAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            var data = _mapper.Map<ArticleUpdateDto>(result.Data.Article);
            var categoryResult = await _categoryService.GetAllAsync(Status.Active, serviceInputDto);
            data.Categories = categoryResult.Data.Categories;
            data.Content = await _imageHelper.ShowReadContent(data.ContentUrl ?? "");
            return View(data);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> Update(ArticleUpdateDto updateDto)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        if (ModelState.IsValid)
        {
            var contentCreateResult = await _imageHelper.CreateContentFolder(updateDto.Content ?? "", updateDto.Title, updateDto.Title, nameof(Article), serviceInputDto);
            if (contentCreateResult?.Data != null && contentCreateResult?.ExeptionStatus == ExeptionStatus.Success)
            {
                await _imageHelper.Delete(updateDto.ContentUrl ?? "", serviceInputDto);
                updateDto.ContentUrl = contentCreateResult.Data.FolderUrl;
                if (updateDto.File != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImgFolder(updateDto.File, updateDto.Title, updateDto.Title, nameof(Article), serviceInputDto);
                    if (imageUploadResult?.Data != null && imageUploadResult.ExeptionStatus == ExeptionStatus.Success)
                    {
                        if (updateDto.ImageUrl != null) { await _imageHelper.Delete(updateDto.ImageUrl, serviceInputDto); }
                        updateDto.ImageUrl = imageUploadResult.Data?.FolderUrl;
                    }
                }
            }
            var result = await _articleService.UpdateAsync(updateDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Güncellendi");
                return RedirectToAction("Index", "Article");
            }
        }
        else
        {
            var categoryResult = await _categoryService.GetAllAsync(Status.Active, serviceInputDto);
            updateDto.Categories = categoryResult.Data.Categories;
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
        var result = await _articleService.ConfirmationAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Durumu Değiştirildi");
            return RedirectToAction("Index", "Article");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    public async Task<IActionResult> Delete(string Id)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _articleService.DeleteAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Silindi");
            return RedirectToAction("Index", "Article");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }
}