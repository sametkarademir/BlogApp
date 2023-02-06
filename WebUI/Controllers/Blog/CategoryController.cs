using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers.Blog;

[Authorize(Roles = "Admin,Blog")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toastNotification;
    private readonly IHttpContextHelper _httpContextHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CategoryController(ICategoryService categoryService, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor)
    {
        _categoryService = categoryService;
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
        var result = await _categoryService.GetAllAsync(null, serviceInputDto);
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
    public async Task<IActionResult> Create(CategoryAddDto addDto)
    {
        if (ModelState.IsValid)
        {
            #region Get HttpContext
            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            #endregion
            var result = await _categoryService.CreateAsync(addDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Kaydedildi");
                return RedirectToAction("Index", "Category");
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
        var result = await _categoryService.GetAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            var data = _mapper.Map<CategoryUpdateDto>(result.Data.Category);
            return View(data);
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateDto updateDto)
    {
        if (ModelState.IsValid)
        {
            #region Get HttpContext
            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            #endregion
            var result = await _categoryService.UpdateAsync(updateDto, serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage("Güncellendi");
                return RedirectToAction("Index", "Category");
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
        var result = await _categoryService.ConfirmationAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Durumu Değiştirildi");
            return RedirectToAction("Index", "Category");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }

    public async Task<IActionResult> Delete(string Id)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _categoryService.DeleteAsync(Id, serviceInputDto);
        if (result.ExeptionStatus == ExeptionStatus.Success)
        {
            _toastNotification.AddSuccessToastMessage("Silindi");
            return RedirectToAction("Index", "Category");
        }
        _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
        return RedirectToAction("Error", "Admin");
    }
}