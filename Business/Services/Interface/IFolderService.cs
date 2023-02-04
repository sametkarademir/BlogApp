using Business.Dtos;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface IFolderService
{
    /// <summary>
    /// Get Folder Service
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<FolderDto>> GetAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Get All Folder Service
    /// </summary>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<FolderListDto>> GetAllAsync(ServiceInputDto serviceInputDto);

    /// <summary>
    /// Get Folder By Find ObjectId and ObjectName Service
    /// </summary>
    /// <param name="objectId">Entity Id</param>
    /// <param name="objectName">Entity</param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<FolderDto>> GetByObjectAsync(string url, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Get All By Find By ObjectId and ObjectName Service
    /// </summary>
    /// <param name="objectId">Entity Id</param>
    /// <param name="objectName">Entity</param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<FolderListDto>> GetAllByObjectAsync(string objectId, string objectName, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Create Folder Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> CreateAsync(FolderAddDto addDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Delete Folder Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> DeleteAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Delete Folder By Find Url Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> DeleteByUrlAsync(string url, ServiceInputDto serviceInputDto);
}