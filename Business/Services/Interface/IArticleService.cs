using Business.Dtos;
using Shared.Entities.Abstract;
using Shared.Utilities.Result;

namespace Business.Services.Interface;

public interface IArticleService
{
    /// <summary>
    /// Get Article Service
    /// </summary>
    /// <param name="id">Entity Id</param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ArticleDto>> GetAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Get All Article Service
    /// </summary>
    /// <param name="status"></param>
    /// <param name="serviceInputDto"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    Task<OperationResult<ArticleListDto>> GetAllAsync(Status? status, ServiceInputDto serviceInputDto, bool include = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="currenPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="isAscending"></param>
    /// <returns></returns>
    Task<OperationResult<ArticleListDto>> GetAllByPages(Status? status, ServiceInputDto serviceInputDto, string? categoryId, int currenPage = 1, int pageSize = 9, bool isAscending = false);

    /// <summary>
    /// Create Article Service
    /// </summary>
    /// <param name="addDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ArticleDto>> CreateAsync(ArticleAddDto addDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Create Article Service
    /// </summary>
    /// <param name="updateDto"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<ArticleDto>> UpdateAsync(ArticleUpdateDto updateDto, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Change Status Article Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> ConfirmationAsync(string id, ServiceInputDto serviceInputDto);

    /// <summary>
    /// Delete Article Service
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serviceInputDto"></param>
    /// <returns></returns>
    Task<OperationResult<bool>> DeleteAsync(string id, ServiceInputDto serviceInputDto);
}