using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Data.Repositories.Interface;
using Entities.Concrete;
using Shared.Entities.Abstract;
using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;

namespace Business.Services.Concrete;

public class ResumeManager : BaseManager, IResumeService
{
    private readonly ISystemLogService _systemLogService;
    public ResumeManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion, ISystemLogService systemLogService) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
        _systemLogService = systemLogService;
    }

    public async Task<OperationResult<ResumeDto>> GetAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ResumeDto> { Data = new ResumeDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.ResumeRepository.GetAsync(item => item.Id == id);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new ResumeDto { Resume = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{result.Data.Title} ünvanlı {nameof(Resume)} çağrıldı";
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

    public async Task<OperationResult<ResumeListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ResumeListDto> { Data = new ResumeListDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            OperationResult<List<Resume>> result;
            if (status == null || status == Status.None) { result = await Uow.ResumeRepository.GetAllAsync(); }
            else { result = await Uow.ResumeRepository.GetAllAsync(item => item.Status == status); }
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new ResumeListDto { Resumes = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"Tüm {nameof(Resume)} listesi çağrıldı";
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

    public async Task<OperationResult<ResumeDto>> CreateAsync(ResumeAddDto addDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ResumeDto> { Data = new ResumeDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var data = Mapper.Map<Resume>(addDto);
            data.CreatedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.CreatedBy = serviceInputDto.Username ?? "System";
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.ResumeRepository.CreateAsync(data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                await Uow.SaveAsync();
                res.Data.Resume = data;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{addDto.Title} ünvanlı {nameof(Resume)} eklendi";

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

    public async Task<OperationResult<ResumeDto>> UpdateAsync(ResumeUpdateDto updateDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ResumeDto> { Data = new ResumeDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        if (String.IsNullOrEmpty(updateDto.Id))
        {
            res.ExeptionStatus = ExeptionStatus.InvalidRequest;
            throw new ArgumentException("id boş veya uygun formatta değil");
        }
        try
        {
            #region Get Resume Request
            var value = await GetAsync(updateDto.Id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Resume == null) { return value; }
            #endregion

            var data = Mapper.Map(updateDto, value.Data.Resume);
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.ResumeRepository.UpdateAsync(entity: data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data.Resume = result.Data;
                res.ExeptionStatus = ExeptionStatus.Success;
                await Uow.SaveAsync();
                logStatus = LogStatus.Success;
                msg = $"{updateDto.Title} ünvanlı {nameof(Resume)} güncellendi";
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
            #region Get Resume Request
            var value = await GetAsync(id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Resume == null)
            {
                msg = $"{value.Exception?.Message} {value.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
            #endregion
            value.Data.Resume.Status = value.Data.Resume.Status == Status.Active ? Status.Disable : Status.Active;
            var result = await Uow.ResumeRepository.UpdateAsync(value.Data.Resume);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{value.Data.Resume.Title} ünvanlı {nameof(Resume)} durumu {value.Data.Resume.Status.ToString()} olarak güncellendi.";
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
            if (data.ExeptionStatus != ExeptionStatus.Success || data.Data.Resume == null)
            {
                res.ExeptionStatus = data.ExeptionStatus;
                throw new ArgumentException($"{data.Exception?.Message} {data.Exception?.StackTrace}");
            }
            var result = await Uow.ResumeRepository.RemoveAsync(data.Data.Resume);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{data.Data.Resume.Title} ünvanlı {nameof(Resume)} silindi";
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