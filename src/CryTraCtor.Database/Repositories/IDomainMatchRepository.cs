using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories
{
    public interface IDomainMatchRepository
    {
        Task<List<DomainMatchEntity>> GetByTrafficParticipantIdAsync(Guid trafficParticipantId);

        Task InsertAsync(DomainMatchEntity entity);

        Task InsertRangeAsync(IEnumerable<DomainMatchEntity> entities);
    }
}
