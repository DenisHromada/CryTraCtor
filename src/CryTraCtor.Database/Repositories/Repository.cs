using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class Repository<TEntity>(
    DbContext dbContext,
    IEntityMapper<TEntity> entityMapper
) : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();
    public IQueryable<TEntity> Get() => _dbSet;

    // This is a hacky solution to get all entities before the db context is saved
    public IQueryable<TEntity> GetLocal() => _dbSet.Local.AsQueryable();

    public async ValueTask<bool> ExistsAsync(TEntity entity)
    {
        if (entity.Id == Guid.Empty)
        {
            return false;
        }

        return await _dbSet.AnyAsync(e => e.Id == entity.Id);
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
        => (await _dbSet.AddAsync(entity)).Entity;

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var existingEntity = await _dbSet.SingleAsync(e => e.Id == entity.Id).ConfigureAwait(false);
        entityMapper.MapToExistingEntity(existingEntity, entity);
        return existingEntity;
    }

    public async Task DeleteAsync(Guid entityId)
        => _dbSet.Remove(await _dbSet.SingleAsync(i => i.Id == entityId).ConfigureAwait(false));
}
