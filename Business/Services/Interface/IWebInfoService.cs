using Business.Dtos;
using Entities.Concrete;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface IWebInfoService
{
    /// <summary>
    /// Get SystemLog Find Bu Id Service
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <param name="serviceInputDto"></param>
    /// <returns>SystemLog</returns>
    Task<OperationResult<WebInfo>> GetAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Update SystemLog Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns>SystemLog</returns>
    Task<OperationResult<WebInfo>> UpdateAsync(WebInfoUpdateDto addDto, ServiceInputDto serviceInputDto);
}