using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class KnownDomainEntityMapper : IEntityMapper<KnownDomainEntity>
{
    public void MapToExistingEntity(KnownDomainEntity existingEntity, KnownDomainEntity newEntity)
    {
        existingEntity.DomainName = newEntity.DomainName;
        existingEntity.Purpose = newEntity.Purpose;
        existingEntity.Description = newEntity.Description;
    }
}