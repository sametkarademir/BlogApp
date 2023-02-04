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

public class ProjectManager : BaseManager, IProjectService
{
    private readonly ISystemLogService _systemLogService;
    public ProjectManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion, ISystemLogService systemLogService) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
        _systemLogService = systemLogService;
    }

    public async Task<OperationResult<ProjectDto>> GetAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ProjectDto> { Data = new ProjectDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.ProjectRepository.GetAsync(item => item.Id == id);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new ProjectDto { Project = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{result.Data.Name} adlı {nameof(Project)} çağrıldı";
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

    public async Task<OperationResult<ProjectListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ProjectListDto> { Data = new ProjectListDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            OperationResult<List<Project>> result;
            if (status == null || status == Status.None) { result = await Uow.ProjectRepository.GetAllAsync(); }
            else { result = await Uow.ProjectRepository.GetAllAsync(item => item.Status == status); }
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = new ProjectListDto { Projects = result.Data };
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"Tüm {nameof(Project)} listesi çağrıldı";
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

    public async Task<OperationResult<ProjectDto>> CreateAsync(ProjectAddDto addDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ProjectDto> { Data = new ProjectDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var data = Mapper.Map<Project>(addDto);
            data.CreatedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.CreatedBy = serviceInputDto.Username ?? "System";
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.ProjectRepository.CreateAsync(data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                await Uow.SaveAsync();
                res.Data.Project = data;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{addDto.Name} adlı {nameof(Project)} eklendi";

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

    public async Task<OperationResult<ProjectDto>> UpdateAsync(ProjectUpdateDto updateDto, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<ProjectDto> { Data = new ProjectDto(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        var logStatus = LogStatus.Error;
        if (String.IsNullOrEmpty(updateDto.Id))
        {
            res.ExeptionStatus = ExeptionStatus.InvalidRequest;
            throw new ArgumentException("id boş veya uygun formatta değil");
        }
        try
        {
            #region Get Project Request
            var value = await GetAsync(updateDto.Id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Project == null) { return value; }
            #endregion

            var data = Mapper.Map(updateDto, value.Data.Project);
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.ProjectRepository.UpdateAsync(entity: data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data.Project = result.Data;
                res.ExeptionStatus = ExeptionStatus.Success;
                await Uow.SaveAsync();
                logStatus = LogStatus.Success;
                msg = $"{updateDto.Name}  adlı {nameof(Project)} güncellendi";
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
            #region Get Project Request
            var value = await GetAsync(id, serviceInputDto);
            if (value.ExeptionStatus != ExeptionStatus.Success || value.Data.Project == null)
            {
                msg = $"{value.Exception?.Message} {value.Exception?.StackTrace}";
                throw new ArgumentException(msg);
            }
            #endregion
            value.Data.Project.Status = value.Data.Project.Status == Status.Active ? Status.Disable : Status.Active;
            var result = await Uow.ProjectRepository.UpdateAsync(value.Data.Project);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{value.Data.Project.Name} adlı {nameof(Project)} durumu {value.Data.Project.Status.ToString()} olarak güncellendi.";
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
            if (data.ExeptionStatus != ExeptionStatus.Success || data.Data.Project == null)
            {
                res.ExeptionStatus = data.ExeptionStatus;
                throw new ArgumentException($"{data.Exception?.Message} {data.Exception?.StackTrace}");
            }
            var result = await Uow.ProjectRepository.RemoveAsync(data.Data.Project);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{data.Data.Project.Name}  adlı {nameof(Project)} silindi";
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