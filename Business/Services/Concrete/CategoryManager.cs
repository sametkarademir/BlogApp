using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Data.Repositories.Interface;
using Entities.Concrete;
using Entities.Concrete.Blog;
using Shared.Entities.Abstract;
using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;

namespace Business.Services.Concrete;

public class CategoryManager : BaseManager, ICategoryService
{
    private readonly ISystemLogService _systemLogService;
    public CategoryManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion, ISystemLogService systemLogService) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
        _systemLogService = systemLogService;
    }

    public async Task<OperationResult<CategoryDto>> GetAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<CategoryDto> { Data = new CategoryDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.CategoryRepository.GetAsync(item => item.Id == id);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new CategoryDto { Category = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{result.Data.Name} adlı {nameof(Category)} çağrıldı";
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

    public async Task<OperationResult<CategoryListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<CategoryListDto> { Data = new CategoryListDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            OperationResult<List<Category>> result;
            if (status == null || status == Status.None) { result = await Uow.CategoryRepository.GetAllAsync(); }
            else { result = await Uow.CategoryRepository.GetAllAsync(item => item.Status == status); }
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new CategoryListDto { Categories = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"Tüm {nameof(Category)} listesi çağrıldı";
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

    public async Task<OperationResult<CategoryDto>> CreateAsync(CategoryAddDto addDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<CategoryDto> { Data = new CategoryDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var data = Mapper.Map<Category>(addDto);
            data.CreatedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.CreatedBy = serviceInputDto.Username ?? "System";
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.CategoryRepository.CreateAsync(data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                await Uow.SaveAsync();
                res.Data.Category = data;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{addDto.Name} adlı {nameof(Category)} eklendi";

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

    public async Task<OperationResult<CategoryDto>> UpdateAsync(CategoryUpdateDto updateDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<CategoryDto> { Data = new CategoryDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        if (String.IsNullOrEmpty(updateDto.Id))
        {
            res.ExeptionStatus = ExeptionStatus.InvalidRequest;
            throw new ArgumentException("id boş veya uygun formatta değil");
        }
        try
        {
            #region Get Category Request
            var value = await GetAsync(updateDto.Id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Category == null) { return value; }
            #endregion

            var data = Mapper.Map(updateDto, value.Data.Category);
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.CategoryRepository.UpdateAsync(entity: data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data.Category = result.Data;
                res.ExeptionStatus = ExeptionStatus.Success;
                await Uow.SaveAsync();
                logStatus = LogStatus.Success;
                msg = $"{updateDto.Name}  adlı {nameof(Category)} güncellendi";
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
            #region Get Category Request
            var value = await GetAsync(id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Category == null)
            {
                msg = $"{value.Exception?.Message} {value.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
            #endregion
            value.Data.Category.Status = value.Data.Category.Status == Status.Active ? Status.Disable : Status.Active;
            var result = await Uow.CategoryRepository.UpdateAsync(value.Data.Category);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{value.Data.Category.Name} adlı {nameof(Category)} durumu {value.Data.Category.Status.ToString()} olarak güncellendi.";
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
            if (data.ExeptionStatus != ExeptionStatus.Success || data.Data.Category == null)
            {
                res.ExeptionStatus = data.ExeptionStatus;
                throw new ArgumentException($"{data.Exception?.Message} {data.Exception?.StackTrace}");
            }
            var result = await Uow.CategoryRepository.RemoveAsync(data.Data.Category);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{data.Data.Category.Name}  adlı {nameof(Category)} silindi";
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