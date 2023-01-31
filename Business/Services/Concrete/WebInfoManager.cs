using AutoMapper;
using Business.Dtos;
using Business.Services.Abstract;
using Business.Services.Interface;
using Data.Repositories.Interface;
using Entities.Concrete;
using Shared.Utilities.Extenstions.Interface;
using Shared.Utilities.Result;

namespace Business.Services.Concrete;

public class WebInfoManager : BaseManager, IWebInfoService
{
    public WebInfoManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion) : base(uow, mapper, dateTimeExtensions, generateBase64SessionIdExtenstion)
    {
    }
    public async Task<OperationResult<WebInfo>> GetAsync(string? id)
    {
        if (id == null) { return new OperationResult<WebInfo> { ExeptionStatus = ExeptionStatus.InvalidRequest }; }
        try { return await Uow.WebInfoRepository.GetAsync(predicate: item => item.Id == id); }
        catch { return new OperationResult<WebInfo> { ExeptionStatus = ExeptionStatus.Error }; }
    }

    public async Task<OperationResult<WebInfo>> UpdateAsync(WebInfoUpdateDto? updateDto)
    {
        if (updateDto == null) { return new OperationResult<WebInfo> { ExeptionStatus = ExeptionStatus.InvalidRequest }; }
        try
        {
            var value = await GetAsync(updateDto.Id);
            if (value.Data == null || value.ExeptionStatus != ExeptionStatus.Success) { return value; }

            var data = Mapper.Map(updateDto, value.Data);
            data.ModifiedAt = DateTimeExtensions.ToUnixTime(DateTime.UtcNow);
            data.ModifiedBy = "System";
            var result = await Uow.WebInfoRepository.UpdateAsync(entity: data);
            if (result.ExeptionStatus == ExeptionStatus.Success) { await Uow.SaveAsync(); }
            return result;
        }
        catch { return new OperationResult<WebInfo> { ExeptionStatus = ExeptionStatus.Error }; }
    }
}