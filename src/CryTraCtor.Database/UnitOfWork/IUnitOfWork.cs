using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories;

namespace CryTraCtor.Database.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();

    IDnsPacketRepository DnsPackets { get; }
    IDomainMatchRepository DomainMatches { get; }
    IBitcoinPacketRepository BitcoinPackets { get; }
    TrafficParticipantAggregateRepository TrafficParticipantAggregates { get; }

    Task CommitAsync();
}
