using Microsoft.EntityFrameworkCore;
using Shared.Data.Interface;
using Shared.Utilities.Result;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shared.Data.Concrete;

public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class, new()
{
    private readonly DbContext _context;
    public EntityRepository(DbContext context) { _context = context; }


    public async Task<OperationResult<int>> Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var res = new OperationResult<int> { Data = 0, ExeptionStatus = ExeptionStatus.Error };
        int data = 0;
        try
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            await Task.Run(() => { data = queryable.Count(); });
            res.Data = data;
            res.ExeptionStatus = ExeptionStatus.Success;
        }
        catch (Exception ex) { res.Exception = ex; }
        return res;
    }

    public async Task<OperationResult<TEntity>> CreateAsync(TEntity entity)
    {
        var res = new OperationResult<TEntity>{Data = entity, ExeptionStatus = ExeptionStatus.Error };
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            res.ExeptionStatus = ExeptionStatus.Success;
        }
        catch (Exception ex) { res.Exception = ex; }
        return res;
    }

    public async Task<OperationResult<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[]? includesProperties)
    {
        var res = new OperationResult<List<TEntity>>{Data = new List<TEntity>(), ExeptionStatus = ExeptionStatus.Error };
        try
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (predicate != null) { queryable = queryable.Where(predicate); }
            if (includesProperties != null && includesProperties.Any()) { foreach (var includesProperty in includesProperties) { queryable = queryable.Include(includesProperty); } }
            var list = await queryable.ToListAsync();
            res.Data = list;
            res.ExeptionStatus = ExeptionStatus.Success;
        }
        catch (Exception ex) { res.Exception = ex; }
        return res;
    }

    public async Task<OperationResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[]? includesProperties)
    {
        var res = new OperationResult<TEntity> { Data = new TEntity(), ExeptionStatus = ExeptionStatus.Error };
        try
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (predicate != null) { queryable = queryable.Where(predicate); }
            if (includesProperties != null && includesProperties.Any()) { foreach (var includesProperty in includesProperties) { queryable = queryable.Include(includesProperty); } }
            var entity = await queryable.SingleOrDefaultAsync();
            res.Data = entity ?? new TEntity();
            res.ExeptionStatus = ExeptionStatus.Success;
        }
        catch (Exception ex) { res.Exception = ex; }
        return res;
    }

    public async Task<OperationResult<TEntity>> RemoveAsync(TEntity entity)
    {
        var res = new OperationResult<TEntity> { Data = new TEntity(), ExeptionStatus = ExeptionStatus.Error };
        try
        {
            await Task.Run(() => { _context.Set<TEntity>().Remove(entity); });
            res.Data = entity;
            res.ExeptionStatus = ExeptionStatus.Success;
        }
        catch (Exception ex) { res.Exception = ex; }
        return res;
    }

    public async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
    {
        var res = new OperationResult<TEntity> { Data = new TEntity(), ExeptionStatus = ExeptionStatus.Error };
        try
        {
            await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
            res.Data = entity;
            res.ExeptionStatus = ExeptionStatus.Success;
        }
        catch (Exception ex) { res.Exception = ex; }
        return res;
    }
}
