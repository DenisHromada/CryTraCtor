using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.UnitOfWork;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    private IDnsPacketRepository? _dnsPackets;
    private IDomainMatchRepository? _domainMatches;
    private IBitcoinPacketRepository? _bitcoinPackets;
    private TrafficParticipantAggregateRepository? _trafficParticipantAggregates;

    public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new()
        => new Repository<TEntity>(_dbContext, new TEntityMapper());

    public IDnsPacketRepository DnsPackets =>
        _dnsPackets ??= new DnsPacketRepository((CryTraCtorDbContext)_dbContext, new DnsPacketEntityMapper());

    public IDomainMatchRepository DomainMatches =>
        _domainMatches ??= new DomainMatchRepository((CryTraCtorDbContext)_dbContext);

    public IBitcoinPacketRepository BitcoinPackets =>
        _bitcoinPackets ??= new BitcoinPacketRepository((CryTraCtorDbContext)_dbContext, new BitcoinPacketEntityMapper());

    public TrafficParticipantAggregateRepository TrafficParticipantAggregates
    {
        get
        {
            return _trafficParticipantAggregates ??=
                new TrafficParticipantAggregateRepository((CryTraCtorDbContext)_dbContext);
        }
    }

    public async Task CommitAsync() => await _dbContext.SaveChangesAsync().ConfigureAwait(false);

    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync().ConfigureAwait(false);
}
