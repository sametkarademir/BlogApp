using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Shared.Utilities.Result;
using WebUI.Helpers.Interface;

namespace WebUI.Controllers.WebSite
{
    [Authorize(Roles = "Admin,Home")]
    public class WebInfoController : Controller
    {
        private readonly IWebInfoService _webInfoService;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public WebInfoController(IWebInfoService webInfoService, IImageHelper imageHelper, IMapper mapper, IToastNotification toastNotification)
        {
            _webInfoService = webInfoService;
            _imageHelper = imageHelper;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _webInfoService.GetAsync("WebInfo");
                if (result.Data != null)
                {
                    var data = _mapper.Map<WebInfoUpdateDto>(result.Data);
                    return View(data);
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
                    return RedirectToAction("Error", "Admin");
                }
            }
            catch
            {
                _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
                return RedirectToAction("Error", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(WebInfoUpdateDto updateDto)
        {
            try
            {
                if (updateDto.File != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImgFolder(updateDto.File, $"WebInfo_{updateDto.Id}");
                    if (imageUploadResult.Data != null)
                    {
                        if (updateDto.ImageUrl != null) { await _imageHelper.Delete(updateDto.ImageUrl); }
                        updateDto.ImageUrl = imageUploadResult.Data?.FolderUrl;
                    }
                }
                var result = await _webInfoService.UpdateAsync(updateDto);
                if (result.ExeptionStatus == ExeptionStatus.Success)
                {
                    _toastNotification.AddSuccessToastMessage(OperationMessages.Created());
                    return RedirectToAction("Index","WebInfo");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
                    return RedirectToAction("Error", "Admin");
                }
            }
            catch
            {
                _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
                return RedirectToAction("Error", "Admin");
            }
        }
    }
}
