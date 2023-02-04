using Shared.Utilities.Result;
using System.Linq.Expressions;

namespace Shared.Data.Interface;

public interface IEntityRepository<T> where T : class, new()
{
    /// <summary>
    /// Get Entity Find By Id Generic Repository
    /// </summary>
    /// <param name="predicate">Condition</param>
    /// <param name="includesProperties"></param>
    /// <returns>Entity</returns>
    Task<OperationResult<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includesProperties);

    /// <summary>
    /// Get All Entity List Generic Repository
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includesProperties"></param>
    /// <returns></returns>
    Task<OperationResult<List<T>>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includesProperties);

    /// <summary>
    /// Count Entity Generic Repository
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<OperationResult<int>> Count(Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// Create Entity Generic Repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<OperationResult<T>> CreateAsync(T entity);

    /// <summary>
    /// Update Entity Generic Repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<OperationResult<T>> UpdateAsync(T entity);

    /// <summary>
    /// Remove Entity Generic Repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<OperationResult<T>> RemoveAsync(T entity);
}
