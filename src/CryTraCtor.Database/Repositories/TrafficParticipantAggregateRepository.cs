using CryTraCtor.Database.Dtos;
using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class TrafficParticipantAggregateRepository(IDbContextFactory<CryTraCtorDbContext> dbContextFactory)
{
    public async Task<IEnumerable<TrafficParticipantAggregateDto>> GetAggregatedByFileAnalysisIdAsync(
        Guid fileAnalysisId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var participantEntities = await dbContext.Set<TrafficParticipantEntity>()
            .Where(tp => tp.FileAnalysisId == fileAnalysisId)
            .ToListAsync();

        if (!participantEntities.Any())
        {
            return Enumerable.Empty<TrafficParticipantAggregateDto>();
        }

        var dnsPacketIds = await dbContext.Set<DnsPacketEntity>()
            .Where(dns => dns.FileAnalysisId == fileAnalysisId)
            .Select(dns => new { dns.Id, dns.SenderId, dns.RecipientId })
            .ToListAsync();

        var dnsPacketGuids = dnsPacketIds.Select(d => d.Id).ToHashSet();
        var domainMatches = await dbContext.Set<DomainMatchEntity>()
            .Where(dm => dnsPacketGuids.Contains(dm.DnsPacketId))
            .Select(dm => new { dm.DnsPacketId, dm.KnownDomainId })
            .ToListAsync();

        var dnsPacketParticipantLookup = dnsPacketIds
            .SelectMany(d => new[]
            {
                new { DnsId = d.Id, ParticipantId = d.SenderId }, new { DnsId = d.Id, ParticipantId = d.RecipientId }
            })
            .Where(x => x.ParticipantId != Guid.Empty)
            .ToLookup(x => x.DnsId, x => x.ParticipantId);

        var matchedDomainCounts = domainMatches
            .SelectMany(dm =>
                dnsPacketParticipantLookup[dm.DnsPacketId]
                    .Select(participantId => new { participantId, dm.KnownDomainId }))
            .GroupBy(x => x.participantId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    UniqueCount = g.Select(x => x.KnownDomainId).Distinct().Count(), TotalCount = g.Count()
                }
            );


        var outgoingCounts = dnsPacketIds
            .Where(dns => dns.SenderId != Guid.Empty)
            .GroupBy(dns => dns.SenderId)
            .ToDictionary(g => g.Key, g => g.Count());

        var incomingCounts = dnsPacketIds
            .Where(dns => dns.RecipientId != Guid.Empty)
            .GroupBy(dns => dns.RecipientId)
            .ToDictionary(g => g.Key, g => g.Count());


        var result = participantEntities.Select(entity =>
        {
            var outgoingDnsCount = outgoingCounts.TryGetValue(entity.Id, out var outCount) ? outCount : 0;
            var incomingDnsCount = incomingCounts.TryGetValue(entity.Id, out var inCount) ? inCount : 0;
            return new TrafficParticipantAggregateDto
            {
                Id = entity.Id,
                Address = entity.Address,
                Port = entity.Port,
                FileAnalysisId = entity.FileAnalysisId,
                OutgoingDnsCount = outgoingDnsCount,
                IncomingDnsCount = incomingDnsCount,

                UniqueMatchedKnownDomainCount =
                    matchedDomainCounts.TryGetValue(entity.Id, out var counts) ? counts.UniqueCount : 0,
                TotalMatchedKnownDomainCount =
                    matchedDomainCounts.TryGetValue(entity.Id, out counts) ? counts.TotalCount : 0
            };
        }).ToList();

        return result;
    }
}
