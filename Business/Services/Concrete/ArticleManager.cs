using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Data.Repositories.Interface;
using Entities.Concrete.Blog;
using Entities.Concrete;
using Shared.Entities.Abstract;
using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;

namespace Business.Services.Concrete;

public class ArticleManager : BaseManager, IArticleService
{
    private readonly ISystemLogService _systemLogService;
    public ArticleManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion, ISystemLogService systemLogService) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
        _systemLogService = systemLogService;
    }

    public async Task<OperationResult<ArticleDto>> GetAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ArticleDto> { Data = new ArticleDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.ArticleRepository.GetAsync(item => item.Id == id);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new ArticleDto { Article = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{result.Data.Title} adlı {nameof(Article)} çağrıldı";
            }
            else
            {
                msg = $"{result.Exception?.Message} {result.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
        }
        catch (Exception ex) { if (String.IsNullOrEmpty(msg)) { msg = $"{ex.Message} {ex.StackTrace}"; } }
        finally
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {
                Date = DateTime.UtcNow,
                LogStatus = logStatus,
                Message = msg,
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }

    public async Task<OperationResult<ArticleListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto, bool include = false)
    {
        var res = new OperationResult<ArticleListDto> { Data = new ArticleListDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            OperationResult<List<Article>> result;
            if (status == null || status == Status.None)
            {
                if (include) { result = await Uow.ArticleRepository.GetAllAsync(null, item => item.Category); }
                else { result = await Uow.ArticleRepository.GetAllAsync(); }
            }
            else
            {
                if (include){ result = await Uow.ArticleRepository.GetAllAsync(item => item.Status == status, item => item.Category); }
                else { result = await Uow.ArticleRepository.GetAllAsync(item => item.Status == status); }
                
            }
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new ArticleListDto { Articles = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"Tüm {nameof(Article)} listesi çağrıldı";
            }
            else
            {
                msg = $"{result.Exception?.Message} {result.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
        }
        catch (Exception ex) { if (String.IsNullOrEmpty(msg)) { msg = $"{ex.Message} {ex.StackTrace}"; } }
        finally
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {
                Date = DateTime.UtcNow,
                LogStatus = logStatus,
                Message = msg,
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }

    public async Task<OperationResult<ArticleListDto>> GetAllByPages(Status? status, ServiceInputDto serviceInputDto, string? categoryId, int currenPage = 1,
        int pageSize = 5, bool isAscending = false)
    {
        var res = new OperationResult<ArticleListDto> { Data = new ArticleListDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var result = categoryId == null
                ? await Uow.ArticleRepository.GetAllAsync(item => item.Status == Status.Active, item => item.Category)
                : await Uow.ArticleRepository.GetAllAsync(item => item.Status == Status.Active && item.CategoryId == categoryId, item => item.Category);

            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                var sortedArticles = isAscending
                    ? result.Data.OrderBy(item => item.CreatedAt).Skip((currenPage - 1) * pageSize).Take(pageSize).ToList()
                    : result.Data.OrderByDescending(item => item.CreatedAt).Skip((currenPage - 1) * pageSize).Take(pageSize).ToList();

                res.Data = new ArticleListDto
                {
                    Articles = sortedArticles,
                    CategoryId = categoryId == null ? null : categoryId,
                    CurrentPage = currenPage,
                    PageSize = pageSize,
                    TotalCount = result.Data.Count
                };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"Tüm {nameof(Article)} listesi çağrıldı";
            }
            else
            {
                msg = $"{result.Exception?.Message} {result.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
        }
        catch (Exception ex) { if (String.IsNullOrEmpty(msg)) { msg = $"{ex.Message} {ex.StackTrace}"; } }
        finally
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {
                Date = DateTime.UtcNow,
                LogStatus = logStatus,
                Message = msg,
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }


    public async Task<OperationResult<ArticleDto>> CreateAsync(ArticleAddDto addDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ArticleDto> { Data = new ArticleDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var data = Mapper.Map<Article>(addDto);
            data.CreatedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.CreatedBy = serviceInputDto.Username ?? "System";
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.ArticleRepository.CreateAsync(data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                await Uow.SaveAsync();
                res.Data.Article = data;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{addDto.Title} adlı {nameof(Article)} eklendi";

            }
            else
            {
                msg = $"{result.Exception?.Message} {result.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
        }
        catch (Exception ex) { if (String.IsNullOrEmpty(msg)) { msg = $"{ex.Message} {ex.StackTrace}"; } }
        finally
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {
                Date = DateTime.UtcNow,
                LogStatus = logStatus,
                Message = msg,
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }

    public async Task<OperationResult<ArticleDto>> UpdateAsync(ArticleUpdateDto updateDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ArticleDto> { Data = new ArticleDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        if (String.IsNullOrEmpty(updateDto.Id))
        {
            res.ExeptionStatus = ExeptionStatus.InvalidRequest;
            throw new ArgumentException("id boş veya uygun formatta değil");
        }
        try
        {
            #region Get Article Request
            var value = await GetAsync(updateDto.Id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Article == null) { return value; }
            #endregion

            var data = Mapper.Map(updateDto, value.Data.Article);
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.ArticleRepository.UpdateAsync(entity: data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data.Article = result.Data;
                res.ExeptionStatus = ExeptionStatus.Success;
                await Uow.SaveAsync();
                logStatus = LogStatus.Success;
                msg = $"{updateDto.Title}  adlı {nameof(Article)} güncellendi";
            }
            else
            {
                msg = $"{result.Exception?.Message} {result.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
        }
        catch (Exception ex) { if (String.IsNullOrEmpty(msg)) { msg = $"{ex.Message} {ex.StackTrace}"; } }
        finally
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {

                Date = DateTime.UtcNow,
                LogStatus = logStatus,
                Message = msg,
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }

        return res;
    }

    public async Task<OperationResult<bool>> ConfirmationAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<bool> { Data = false, ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        try
        {
            #region Get Article Request
            var value = await GetAsync(id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Article == null)
            {
                msg = $"{value.Exception?.Message} {value.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
            #endregion
            value.Data.Article.Status = value.Data.Article.Status == Status.Active ? Status.Disable : Status.Active;
            var result = await Uow.ArticleRepository.UpdateAsync(value.Data.Article);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{value.Data.Article.Title} adlı {nameof(Article)} durumu {value.Data.Article.Status.ToString()} olarak güncellendi.";
            }
            else
            {
                msg = $"{result.Exception?.Message} {result.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
        }
        catch (Exception ex) { if (String.IsNullOrEmpty(msg)) { msg = $"{ex.Message} {ex.StackTrace}"; } }
        finally
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {

                Date = DateTime.UtcNow,
                LogStatus = logStatus,
                Message = msg,
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }
    public async Task<OperationResult<bool>> DeleteAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<bool> { Data = false, ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var data = await GetAsync(id, serviceInputDto);
            if (data.ExeptionStatus != ExeptionStatus.Success || data.Data.Article == null)
            {
                res.ExeptionStatus = data.ExeptionStatus;
                throw new ArgumentException($"{data.Exception?.Message} {data.Exception?.StackTrace}");
            }
            var result = await Uow.ArticleRepository.RemoveAsync(data.Data.Article);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{data.Data.Article.Title}  adlı {nameof(Article)} silindi";
            }
            else
            {
                msg = $"{result.Exception?.Message} {result.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
        }
        catch (Exception ex) { if (String.IsNullOrEmpty(msg)) { msg = $"{ex.Message} {ex.StackTrace}"; } }
        finally
        {
            await _systemLogService.CreateAsync(new SystemLogAddDto
            {
                Date = DateTime.UtcNow,
                LogStatus = logStatus,
                Message = msg,
                Method = serviceInputDto.RemoteAction,
                Action = serviceInputDto.RemoteController,
                RemoteAddress = serviceInputDto.RemoteAddress,
                RemotePort = serviceInputDto.RemotePort,
                Username = serviceInputDto.Username
            });
        }
        return res;
    }
}