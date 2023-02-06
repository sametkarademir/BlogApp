using Business.Dtos;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface ICategoryService
{
    /// <summary>
    /// Get Category Service
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<CategoryDto>> GetAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Get All Category Service
    /// </summary>
    /// <param name="status"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<CategoryListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Create Category Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<CategoryDto>> CreateAsync(CategoryAddDto addDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Create Category Service
    /// </summary>
    /// <param name="updateDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<CategoryDto>> UpdateAsync(CategoryUpdateDto updateDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Change Status Category Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> ConfirmationAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Delete Category Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> DeleteAsync(string id, ServiceInputDto serviceInputDto);
}