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

public class WebInfoManager : BaseManager, IWebInfoService
{
    private readonly ISystemLogService _systemLogService;
    public WebInfoManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion, ISystemLogService systemLogService) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
        _systemLogService = systemLogService;
    }

    public async Task<OperationResult<WebInfo>> GetAsync(string id, ServiceInputDto serviceInputDto)
    {
        var res = new OperationResult<WebInfo> { Data = new WebInfo(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            var result = await Uow.WebInfoRepository.GetAsync(predicate: item => item.Id == id);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = result.Data;
                res.ExeptionStatus = ExeptionStatus.Success;
                logStatus = LogStatus.Success;
                msg = $"{id} kodlu {nameof(WebInfo)} çağrıldı";
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

    public async Task<OperationResult<WebInfo>> UpdateAsync(WebInfoUpdateDto updateDto, ServiceInputDto serviceInputDto)
    {
        #region Validation

        if (String.IsNullOrEmpty(updateDto.Id)) { return new OperationResult<WebInfo> { ExeptionStatus = ExeptionStatus.InvalidRequest }; }

        #endregion
        var res = new OperationResult<WebInfo> { Data = new WebInfo(), ExeptionStatus = ExeptionStatus.Error };
        string? msg = null;
        LogStatus logStatus = LogStatus.Error;
        try
        {
            #region Get WebInfo Request

            var value = await Uow.WebInfoRepository.GetAsync(predicate: item => item.Id == updateDto.Id);
            if (value.ExeptionStatus != ExeptionStatus.Success) { return value; }

            #endregion

            var data = Mapper.Map(updateDto, value.Data);
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = serviceInputDto.Username ?? "System";
            var result = await Uow.WebInfoRepository.UpdateAsync(entity: data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = result.Data;
                res.ExeptionStatus = ExeptionStatus.Success;
                await Uow.SaveAsync();
                logStatus = LogStatus.Success;
                msg = $"{updateDto.Name} adlı {nameof(WebInfo)} güncellendi";
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