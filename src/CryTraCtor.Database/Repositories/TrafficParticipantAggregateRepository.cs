using CryTraCtor.Database.Dtos;
using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

// Inject IDbContextFactory instead of DbContext
public class TrafficParticipantAggregateRepository(IDbContextFactory<CryTraCtorDbContext> dbContextFactory)
{

    public async Task<IEnumerable<TrafficParticipantAggregateDto>> GetAggregatedByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        // Create DbContext instance from factory
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // Fetch participants first using the created context
        var participantEntities = await dbContext.Set<TrafficParticipantEntity>()
            .Where(tp => tp.FileAnalysisId == fileAnalysisId)
            .ToListAsync();

        if (!participantEntities.Any())
        {
            return Enumerable.Empty<TrafficParticipantAggregateDto>();
        }

        // Fetch DNS packet sender/recipient IDs second using the created context
        var dnsPacketIds = await dbContext.Set<DnsPacketEntity>()
            .Where(dns => dns.FileAnalysisId == fileAnalysisId)
            .Select(dns => new { dns.SenderId, dns.RecipientId })
            .ToListAsync();

        // Perform aggregation in memory
        var outgoingCounts = dnsPacketIds
            .Where(dns => dns.SenderId != Guid.Empty)
            .GroupBy(dns => dns.SenderId)
            .ToDictionary(g => g.Key, g => g.Count());

        var incomingCounts = dnsPacketIds
             .Where(dns => dns.RecipientId != Guid.Empty)
            .GroupBy(dns => dns.RecipientId)
            .ToDictionary(g => g.Key, g => g.Count());

        // Map to DTO and add counts
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
                IncomingDnsCount = incomingDnsCount
            };
        }).ToList();

        return result;
    }
}
