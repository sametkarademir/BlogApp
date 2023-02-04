using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Data.Repositories.Interface;
using Entities.Concrete;
using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;

namespace Business.Services.Concrete;

public class SystemLogManager : BaseManager, ISystemLogService
{
    public SystemLogManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
    }

    public async Task<OperationResult<SystemLogListDto>> GetAllAsync()
    {
        var res = new OperationResult<SystemLogListDto> { Data = null, ExeptionStatus = ExeptionStatus.Error };
        try
        {
            var result = await Uow.SystemLogRepository.GetAllAsync();
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.ExeptionStatus = result.ExeptionStatus;
                res.Data = new SystemLogListDto { SystemLogList = result.Data };
            }
        }
        catch { }
        return res;
    }

    public async Task<OperationResult<RequestCountListDto>> GetAllRequestCountAsync()
    {
        var res = new OperationResult<RequestCountListDto> { Data = null, ExeptionStatus = ExeptionStatus.Error };
        try
        {
            var result = await Uow.SystemLogRepository.GetAllAsync();
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                var groupList = result.Data.GroupBy(item => item.Action).Select(x => new RequestCountDto { Key = x.Key, Count = x.Count() });
                res.ExeptionStatus = result.ExeptionStatus;
                res.Data = new RequestCountListDto { RequestCountDtos = groupList };
            }
        }
        catch { }
        return res;
    }

    public async Task<OperationResult<bool>> CreateAsync(SystemLogAddDto addDto)
    {
        var res = new OperationResult<bool> { Data = false, ExeptionStatus = ExeptionStatus.Error };
        try
        {
            var data = Mapper.Map<SystemLog>(addDto);
            var result = await Uow.SystemLogRepository.CreateAsync(data);
            if (result.ExeptionStatus == ExeptionStatus.Success)
            {
                res.Data = true;
                res.ExeptionStatus = result.ExeptionStatus;
                await Uow.SaveAsync();
            }
        }
        catch { }
        return res;
    }
}