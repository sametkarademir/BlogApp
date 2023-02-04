using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Data.Repositories.Interface;
using Entities.Concrete;
using Shared.Entities.Abstract;
using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;
using System.Security.AccessControl;
using System;

namespace Business.Services.Concrete;

public class FolderManager : BaseManager, IFolderService
{
    private readonly ISystemLogService _systemLogService;
    public FolderManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion, ISystemLogService systemLogService) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
        _systemLogService = systemLogService;
    }


    public async Task<OperationResult<FolderDto>> GetAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<FolderDto> { Data = new FolderDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.FolderRepository.GetAsync(item => item.Id == id);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new FolderDto { Folder = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{result.Data.Name} adlı {nameof(Folder)} çağrıldı";
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

    public async Task<OperationResult<FolderListDto>> GetAllAsync(ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<FolderListDto> { Data = new FolderListDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.FolderRepository.GetAllAsync();
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new FolderListDto { Folders = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"Tüm {nameof(Folder)} listesi çağrıldı";
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

    public async Task<OperationResult<FolderDto>> GetByObjectAsync(string url, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<FolderDto> { Data = new FolderDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.FolderRepository.GetAsync(item => item.Url == url);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new FolderDto { Folder = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{result.Data.Url} yolunda ki {nameof(Folder)} çağrıldı";
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

    public async Task<OperationResult<FolderListDto>> GetAllByObjectAsync(string objectId, string objectName, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<FolderListDto> { Data = new FolderListDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.FolderRepository.GetAllAsync(item => item.ObjectId == objectId && item.ObjectName == objectName);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new FolderListDto { Folders = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{objectId} kodlu ve {objectName} ait {nameof(Folder)} listesi çağrıldı";
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

    public async Task<OperationResult<bool>> CreateAsync(FolderAddDto addDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<bool> { Data = false, ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var data = Mapper.Map<Folder>(addDto);
            data.CreatedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.CreatedBy = serviceInputDto.Username ?? "System";
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.FolderRepository.CreateAsync(data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                await Uow.SaveAsync();
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{addDto.Name} adlı {nameof(Folder)} eklendi";
                
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
            if (data.ExeptionStatus != ExeptionStatus.Success || data.Data.Folder == null)
            {
                res.Data = false;
                res.ExeptionStatus = data.ExeptionStatus;
                throw new ArgumentException($"{data.Exception?.Message} {data.Exception?.StackTrace}");
            }
            var result = await Uow.FolderRepository.RemoveAsync(data.Data.Folder);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{data.Data.Folder.Name} adlı {nameof(Folder)} silindi";
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

    public async Task<OperationResult<bool>> DeleteByUrlAsync(string url, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<bool> { Data = false, ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var data = await Uow.FolderRepository.GetAsync(item => item.Url == url);
            if (data.ExeptionStatus != ExeptionStatus.Success || data.Data == null)
            {
                res.Data = false;
                res.ExeptionStatus = data.ExeptionStatus;
                throw new ArgumentException($"{data.Exception?.Message} {data.Exception?.StackTrace}");
            }
            var result = await Uow.FolderRepository.RemoveAsync(data.Data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{data.Data.Name} adlı {nameof(Folder)} silindi";
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