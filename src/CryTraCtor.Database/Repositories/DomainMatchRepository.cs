using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class DomainMatchRepository(CryTraCtorDbContext dbContext) : IDomainMatchRepository
{
    private readonly CryTraCtorDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<List<DomainMatchEntity>> GetByTrafficParticipantIdAsync(Guid trafficParticipantId)
    {
        return await _dbContext.DomainMatch
            .Include(dm => dm.DnsPacket)
            .Include(dm => dm.KnownDomain)
            .ThenInclude(kd => kd!.CryptoProduct)
            .Where(dm => dm.DnsPacket != null &&
                         (dm.DnsPacket.SenderId == trafficParticipantId ||
                          dm.DnsPacket.RecipientId == trafficParticipantId))
            .ToListAsync();
    }

    public async Task<List<DomainMatchEntity>> GetByPacketIdsAsync(IEnumerable<Guid> packetIds)
    {
        var packetIdList = packetIds.ToList();
        if (!packetIdList.Any())
        {
            return new List<DomainMatchEntity>();
        }

        return await _dbContext.DomainMatch
            .Include(dm => dm.KnownDomain)
            .Where(dm => packetIdList.Contains(dm.DnsPacketId))
            .ToListAsync();
    }

    public async Task InsertAsync(DomainMatchEntity entity)
    {
        await _dbContext.DomainMatch.AddAsync(entity);
    }

    public async Task InsertRangeAsync(IEnumerable<DomainMatchEntity> entities)
    {
        await _dbContext.DomainMatch.AddRangeAsync(entities);
    }
}
