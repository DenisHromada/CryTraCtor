using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories.Interfaces
{
    public interface IDomainMatchRepository
    {
        Task<List<DomainMatchEntity>> GetByTrafficParticipantIdAsync(Guid trafficParticipantId);
        Task<List<DomainMatchEntity>> GetByMessageIdsAsync(IEnumerable<Guid> messageIds);

        Task InsertAsync(DomainMatchEntity entity);

        Task InsertRangeAsync(IEnumerable<DomainMatchEntity> entities);
    }
}
