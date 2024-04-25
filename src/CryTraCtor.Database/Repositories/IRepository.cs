using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public interface IRepository<TEntity>
where TEntity : class, IEntity
{
    IQueryable<TEntity> Get();
    IQueryable<TEntity> GetLocal();
    Task DeleteAsync(Guid entityId);
    ValueTask<bool> ExistsAsync(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}