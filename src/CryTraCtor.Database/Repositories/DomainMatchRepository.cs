using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories
{
    public class DomainMatchRepository(IDbContextFactory<CryTraCtorDbContext> dbContextFactory)
        : IDomainMatchRepository
    {
        private readonly IDbContextFactory<CryTraCtorDbContext> _dbContextFactory =
            dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

        public async Task<List<DomainMatchEntity>> GetByTrafficParticipantIdAsync(Guid trafficParticipantId)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await dbContext.DomainMatch
                .Include(dm => dm.DnsPacket)
                .Include(dm => dm.KnownDomain)
                .ThenInclude(kd => kd!.CryptoProduct)
                .Where(dm => dm.DnsPacket != null &&
                             (dm.DnsPacket.SenderId == trafficParticipantId ||
                              dm.DnsPacket.RecipientId == trafficParticipantId))
                .ToListAsync();
        }

        public async Task InsertAsync(DomainMatchEntity entity)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.DomainMatch.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<DomainMatchEntity> entities)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.DomainMatch.AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }
    }
}
