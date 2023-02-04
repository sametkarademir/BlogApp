using Business.Dtos;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface IProjectService
{
    /// <summary>
    /// Get Project Service
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ProjectDto>> GetAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Get All Project Service
    /// </summary>
    /// <param name="status"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ProjectListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Create Project Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ProjectDto>> CreateAsync(ProjectAddDto addDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Create Project Service
    /// </summary>
    /// <param name="updateDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ProjectDto>> UpdateAsync(ProjectUpdateDto updateDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Change Status Project Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> ConfirmationAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Delete Project Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> DeleteAsync(string id, ServiceInputDto serviceInputDto);
}