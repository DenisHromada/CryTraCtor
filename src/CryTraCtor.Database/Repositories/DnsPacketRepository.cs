using CryTraCtor.Database.Dtos;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class DnsPacketRepository(CryTraCtorDbContext dbContext, IEntityMapper<DnsPacketEntity> entityMapper)
    : Repository<DnsPacketEntity>(dbContext, entityMapper), IDnsPacketRepository
{
    public async Task<IEnumerable<DnsPacketEntity>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        return await dbContext.Set<DnsPacketEntity>()
            .Include(e => e.Sender)
            .Include(e => e.Recipient)
            .Where(e => e.FileAnalysisId == fileAnalysisId)
            .ToListAsync();
    }

    public async Task<IEnumerable<DnsPacketFlatPurposeDto>> GetPacketsWithFlatPurposeByFileAnalysisIdAsync(
        Guid fileAnalysisId)
    {
        var query = dbContext.Set<DnsPacketEntity>()
            .AsNoTracking()
            .Include(p => p.Sender)
            .Include(p => p.Recipient)
            .Where(packet => packet.FileAnalysisId == fileAnalysisId)
            .GroupJoin(dbContext.Set<DomainMatchEntity>().AsNoTracking(), packet => packet.Id,
                match => match.DnsPacketId,
                (packet, packetMatches) => new { packet, packetMatches })
            .SelectMany(t => t.packetMatches.DefaultIfEmpty(), (t, match) => new { t, match })
            .GroupJoin(dbContext.Set<KnownDomainEntity>().AsNoTracking(), t => t.match.KnownDomainId,
                knownDomain => knownDomain.Id, (t, domainMatches) => new { t, domainMatches })
            .SelectMany(t => t.domainMatches.DefaultIfEmpty(), (t, knownDomain) => new DnsPacketFlatPurposeDto
            {
                // Map DnsPacketEntity fields
                Id = t.t.t.packet.Id,
                Timestamp = t.t.t.packet.Timestamp,
                TransactionId = t.t.t.packet.TransactionId,
                QueryName = t.t.t.packet.QueryName,
                QueryType = t.t.t.packet.QueryType,
                IsQuery = t.t.t.packet.IsQuery,
                ResponseAddresses = t.t.t.packet.ResponseAddresses,
                FileAnalysisId = t.t.t.packet.FileAnalysisId,

                // Map Sender fields
                SenderId = t.t.t.packet.SenderId,
                SenderAddress = t.t.t.packet.Sender != null ? t.t.t.packet.Sender.Address : null,
                SenderPort = t.t.t.packet.Sender != null ? t.t.t.packet.Sender.Port : null,

                // Map Recipient fields
                RecipientId = t.t.t.packet.RecipientId,
                RecipientAddress = t.t.t.packet.Recipient != null ? t.t.t.packet.Recipient.Address : null,
                RecipientPort = t.t.t.packet.Recipient != null ? t.t.t.packet.Recipient.Port : null,

                // Select the single purpose (can be null)
                Purpose = knownDomain == null ? null : knownDomain.Purpose
            });

        return await query.ToListAsync();
    }
}
