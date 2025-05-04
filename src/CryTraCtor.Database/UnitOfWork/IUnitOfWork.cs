using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories;
using CryTraCtor.Database.Repositories.Interfaces;

namespace CryTraCtor.Database.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();

    IDnsPacketRepository DnsPackets { get; }
    IDomainMatchRepository DomainMatches { get; }
    IBitcoinPacketRepository BitcoinPackets { get; }
    IBitcoinInventoryRepository BitcoinInventories { get; }
    IBitcoinPacketInventoryRepository BitcoinPacketInventories { get; }
    IBitcoinTransactionRepository BitcoinTransactions { get; }
    IBitcoinBlockHeaderRepository BitcoinBlockHeaders { get; }
    TrafficParticipantAggregateRepository TrafficParticipantAggregates { get; }

    Task CommitAsync();
}
