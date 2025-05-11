using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class DomainMatchRepository(CryTraCtorDbContext dbContext) : IDomainMatchRepository
{
    private readonly CryTraCtorDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<List<DomainMatchEntity>> GetByTrafficParticipantIdAsync(Guid trafficParticipantId)
    {
        return await _dbContext.DomainMatch
            .Include(dm => dm.DnsMessage)
            .Include(dm => dm.KnownDomain)
            .ThenInclude(kd => kd!.CryptoProduct)
            .Where(dm => dm.DnsMessage != null &&
                         (dm.DnsMessage.SenderId == trafficParticipantId ||
                          dm.DnsMessage.RecipientId == trafficParticipantId))
            .ToListAsync();
    }

    public async Task<List<DomainMatchEntity>> GetByMessageIdsAsync(IEnumerable<Guid> messageIds)
    {
        var messageIdList = messageIds.ToList();
        if (messageIdList.Count == 0)
        {
            return [];
        }

        return await _dbContext.DomainMatch
            .Include(dm => dm.KnownDomain)
            .Where(dm => messageIdList.Contains(dm.DnsMessageId))
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

    public async Task<List<DomainMatchEntity>> GetByKnownDomainIdAsync(Guid knownDomainId)
    {
        return await _dbContext.DomainMatch
            .Where(dm => dm.KnownDomainId == knownDomainId)
            .ToListAsync();
    }
}
