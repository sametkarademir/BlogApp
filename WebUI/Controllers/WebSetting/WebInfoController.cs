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

namespace WebUI.Controllers.WebSetting
{
    [Authorize(Roles = "Admin,Home")]
    public class WebInfoController : Controller
    {
        private readonly IWebInfoService _webInfoService;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebInfoController(IWebInfoService webInfoService, IImageHelper imageHelper, IMapper mapper, IToastNotification toastNotification, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor)
        {
            _webInfoService = webInfoService;
            _imageHelper = imageHelper;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _httpContextHelper = httpContextHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get WebInfo Controller
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            #region Get HttpContext

            var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
            #endregion
            var result = await _webInfoService.GetAsync("WebInfo", serviceInputDto);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                var data = _mapper.Map<WebInfoUpdateDto>(result.Data);
                return View(data);
            }
            _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
            return RedirectToAction("Error", "Admin");
        }

        /// <summary>
        /// Update WebInfo Controller
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(WebInfoUpdateDto updateDto)
        {

            if (ModelState.IsValid)
            {
                #region Get HttpContext

                var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);

                #endregion
                #region Folder Upload and Delete

                if (updateDto.File != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImgFolder(updateDto.File, $"WebInfo_{updateDto.Id}", updateDto.Id ?? "WebInfo", nameof(WebInfo), serviceInputDto);
                    if (imageUploadResult?.Data != null && imageUploadResult.ExeptionStatus == ExeptionStatus.Success)
                    {
                        if (updateDto.ImageUrl != null) { await _imageHelper.Delete(updateDto.ImageUrl, serviceInputDto); }
                        updateDto.ImageUrl = imageUploadResult.Data?.FolderUrl;
                    }
                    var result = await _webInfoService.UpdateAsync(updateDto, serviceInputDto);
                    _toastNotification.AddSuccessToastMessage(OperationMessages.Created());
                    return RedirectToAction("Index", "WebInfo");
                }
                #endregion
            }
            _toastNotification.AddErrorToastMessage(OperationMessages.ErrorMessage());
            return RedirectToAction("Error", "Admin");
        }
    }
}
