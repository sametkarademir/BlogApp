using Business.Dtos;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface IResumeService
{
    /// <summary>
    /// Get Resume Service
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ResumeDto>> GetAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Get All Resume Service
    /// </summary>
    /// <param name="status"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ResumeListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto);
    
    /// <summary>
    /// Create Resume Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ResumeDto>> CreateAsync(ResumeAddDto addDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Create Resume Service
    /// </summary>
    /// <param name="updateDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ResumeDto>> UpdateAsync(ResumeUpdateDto updateDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Change Status Resume Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> ConfirmationAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Delete Resume Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> DeleteAsync(string id, ServiceInputDto serviceInputDto);

}