using Business.Dtos;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface ISystemLogService
{
    /// <summary>
    /// Get All SystemLog List Service
    /// </summary>
    /// <returns></returns>
    Task<OperationResult<SystemLogListDto>> GetAllAsync();

    /// <summary>
    /// Get All SystemLog List Service
    /// </summary>
    /// <returns></returns>
    Task<OperationResult<RequestCountListDto>> GetAllRequestCountAsync();

    /// <summary>
    /// Create SystemLog Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> CreateAsync(SystemLogAddDto addDto);
}