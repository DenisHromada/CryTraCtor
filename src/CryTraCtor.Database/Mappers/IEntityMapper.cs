using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public interface IEntityMapper<in TEntity>
    where TEntity : IEntity
{
    void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
}