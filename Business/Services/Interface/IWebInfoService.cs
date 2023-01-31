using Business.Dtos;
using Entities.Concrete;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface IWebInfoService
{
    Task<OperationResult<WebInfo>> GetAsync(string? id);
    Task<OperationResult<WebInfo>> UpdateAsync(WebInfoUpdateDto? addDto);
}