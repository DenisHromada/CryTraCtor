using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class DomainMatchEntityMapper
{
    public void UpdateMatchType(DomainMatchEntity entityToUpdate, DomainMatchEntity sourceEntity)
    {
        entityToUpdate.MatchType = sourceEntity.MatchType;
    }
}
