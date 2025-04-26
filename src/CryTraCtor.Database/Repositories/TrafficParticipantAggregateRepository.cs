using CryTraCtor.Database.Dtos;
using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class TrafficParticipantAggregateRepository(CryTraCtorDbContext dbContext)
{
    private readonly CryTraCtorDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<IEnumerable<TrafficParticipantAggregateDto>> GetAggregatedByFileAnalysisIdAsync(
        Guid fileAnalysisId)
    {
        var results = await _dbContext.Set<TrafficParticipantEntity>()
            .AsNoTracking()
            .Where(tp => tp.FileAnalysisId == fileAnalysisId)
            .Select(tp => new TrafficParticipantAggregateDto
            {
                Id = tp.Id,
                Address = tp.Address,
                Port = tp.Port,
                FileAnalysisId = tp.FileAnalysisId,

                OutgoingDnsCount = _dbContext.Set<DnsPacketEntity>()
                    .Count(dns => dns.FileAnalysisId == fileAnalysisId && dns.SenderId == tp.Id),
                IncomingDnsCount = _dbContext.Set<DnsPacketEntity>()
                    .Count(dns => dns.FileAnalysisId == fileAnalysisId && dns.RecipientId == tp.Id),

                OutgoingBitcoinCount = _dbContext.Set<BitcoinPacketEntity>()
                    .Count(btc => btc.FileAnalysisId == fileAnalysisId && btc.SenderId == tp.Id),
                IncomingBitcoinCount = _dbContext.Set<BitcoinPacketEntity>()
                    .Count(btc => btc.FileAnalysisId == fileAnalysisId && btc.RecipientId == tp.Id),

                TotalMatchedKnownDomainCount = _dbContext.Set<DomainMatchEntity>()
                    .Count(dm => _dbContext.Set<DnsPacketEntity>()
                        .Where(dns =>
                            dns.FileAnalysisId == fileAnalysisId && (dns.SenderId == tp.Id || dns.RecipientId == tp.Id))
                        .Select(dns => dns.Id)
                        .Contains(dm.DnsPacketId)),

                UniqueMatchedKnownDomainCount = _dbContext.Set<DomainMatchEntity>()
                    .Where(dm => _dbContext.Set<DnsPacketEntity>()
                        .Where(dns =>
                            dns.FileAnalysisId == fileAnalysisId && (dns.SenderId == tp.Id || dns.RecipientId == tp.Id))
                        .Select(dns => dns.Id)
                        .Contains(dm.DnsPacketId))
                    .Select(dm => dm.KnownDomainId)
                    .Distinct()
                    .Count()
            })
            .ToListAsync();

        return results;
    }
}
