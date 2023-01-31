using Microsoft.EntityFrameworkCore;
using Shared.Data.Interface;
using Shared.Utilities.Result;
using System.Linq.Expressions;

namespace Shared.Data.Concrete;

public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class, new()
{
    private readonly DbContext _context;
    public EntityRepository(DbContext context) { _context = context; }

    public async Task<OperationResult<int>> Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        try
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (predicate != null) { queryable = queryable.Where(predicate); }
            return new OperationResult<int> { Data = await queryable.CountAsync(), ExeptionStatus = ExeptionStatus.Success, };
        }
        catch (Exception ex) { return new OperationResult<int> { ExeptionStatus = ExeptionStatus.Error, Exception = ex, }; }
    }

    public async Task<OperationResult<TEntity>> CreateAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return new OperationResult<TEntity> { Data = entity, ExeptionStatus = ExeptionStatus.Success };
        }
        catch (Exception ex) { return new OperationResult<TEntity> { Data = entity, ExeptionStatus = ExeptionStatus.Error, Exception = ex}; }
    }

    public async Task<OperationResult<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[]? includesProperties)
    {
        try
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (predicate != null) { queryable = queryable.Where(predicate); }
            if (includesProperties != null && includesProperties.Any()) { foreach (var includesProperty in includesProperties) { queryable = queryable.Include(includesProperty); } }
            var list = await queryable.ToListAsync();
            return new OperationResult<List<TEntity>>() { Data= list, ExeptionStatus= ExeptionStatus.Success };
        }
        catch (Exception ex) { return new OperationResult<List<TEntity>> { ExeptionStatus = ExeptionStatus.Error, Exception = ex }; }
    }

    public async Task<OperationResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[]? includesProperties)
    {
        try
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (predicate != null) { queryable = queryable.Where(predicate); }
            if (includesProperties != null && includesProperties.Any()) { foreach (var includesProperty in includesProperties) { queryable = queryable.Include(includesProperty); } }
            var entity = await queryable.SingleOrDefaultAsync();
            return new OperationResult<TEntity> { Data= entity, ExeptionStatus = ExeptionStatus.Success };
        }
        catch (Exception ex) { return new OperationResult<TEntity> { ExeptionStatus = ExeptionStatus.Error, Exception = ex }; }
    }

    public async Task<OperationResult<TEntity>> RemoveAsync(TEntity entity)
    {
        try
        {
            await Task.Run(() => { _context.Set<TEntity>().Remove(entity); });
            return new OperationResult<TEntity> { Data = entity, ExeptionStatus = ExeptionStatus.Success };
        }
        catch (Exception ex) { return new OperationResult<TEntity> { ExeptionStatus = ExeptionStatus.Error, Exception = ex }; }
    }

    public async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
    {
        try
        {
            await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
            return new OperationResult<TEntity> { Data = entity, ExeptionStatus = ExeptionStatus.Success };
        }
        catch (Exception ex) { return new OperationResult<TEntity> { ExeptionStatus = ExeptionStatus.Error, Exception = ex }; }
    }
}
