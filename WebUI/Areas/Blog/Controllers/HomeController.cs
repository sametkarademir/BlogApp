using AutoMapper;
using Business.Dtos;
using Business.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities.Abstract;
using WebUI.Helpers.Interface;

namespace WebUI.Areas.Blog.Controllers;

[Area("Blog")]
public class HomeController : Controller
{
    private readonly IArticleService _articleService;
    private readonly IHttpContextHelper _httpContextHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IImageHelper _imageHelper;
    private readonly IMapper _mapper;

    public HomeController(IArticleService articleService, IHttpContextHelper httpContextHelper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper, IMapper mapper)
    {
        _articleService = articleService;
        _httpContextHelper = httpContextHelper;
        _httpContextAccessor = httpContextAccessor;
        _imageHelper = imageHelper;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string? categoryId, int currentPage = 1, int pageSize = 9)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion

        var result = await _articleService.GetAllByPages(Status.Active, serviceInputDto, categoryId, currentPage, pageSize, false);
        return View(result.Data);
    }

    public async Task<IActionResult> Detail(string Id)
    {
        #region Get HttpContext
        var serviceInputDto = await _httpContextHelper.GetHttpContextContexObject(_httpContextAccessor);
        #endregion
        var result = await _articleService.GetAsync(Id, serviceInputDto);
        var data = _mapper.Map<ArticleUpdateDto>(result.Data.Article);
        data.Content = await _imageHelper.ShowReadContent(data.ContentUrl);
        return View(data);
    }
}