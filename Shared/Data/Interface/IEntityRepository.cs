using Shared.Utilities.Result;
using System.Linq.Expressions;

namespace Shared.Data.Interface;

public interface IEntityRepository<T> where T : class, new()
{
    Task<OperationResult<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includesProperties);
    Task<OperationResult<List<T>>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includesProperties);
    Task<OperationResult<int>> Count(Expression<Func<T, bool>>? predicate = null);
    Task<OperationResult<T>> CreateAsync(T entity);
    Task<OperationResult<T>> UpdateAsync(T entity);
    Task<OperationResult<T>> RemoveAsync(T entity);
}
