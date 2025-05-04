using CryTraCtor.Database.Dtos;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class DnsMessageRepository(CryTraCtorDbContext dbContext, IEntityMapper<DnsMessageEntity> entityMapper)
    : Repository<DnsMessageEntity>(dbContext, entityMapper), IDnsMessageRepository
{
    public async Task<IEnumerable<DnsMessageEntity>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        return await dbContext.Set<DnsMessageEntity>()
            .Include(e => e.Sender)
            .Include(e => e.Recipient)
            .Where(e => e.FileAnalysisId == fileAnalysisId)
            .ToListAsync();
    }

    public async Task<IEnumerable<DnsMessageFlatPurposeDto>> GetMessagesWithFlatPurposeByFileAnalysisIdAsync(
        Guid fileAnalysisId)
    {
        var query = dbContext.Set<DnsMessageEntity>()
            .AsNoTracking()
            .Include(p => p.Sender)
            .Include(p => p.Recipient)
            .Where(message => message.FileAnalysisId == fileAnalysisId)
            .GroupJoin(dbContext.Set<DomainMatchEntity>().AsNoTracking(), message => message.Id,
                match => match.DnsMessageId,
                (message, messageMatches) => new { message, messageMatches })
            .SelectMany(t => t.messageMatches.DefaultIfEmpty(), (t, match) => new { t, match })
            .GroupJoin(dbContext.Set<KnownDomainEntity>().AsNoTracking(), t => t.match.KnownDomainId,
                knownDomain => knownDomain.Id, (t, domainMatches) => new { t, domainMatches })
            .SelectMany(t => t.domainMatches.DefaultIfEmpty(), (t, knownDomain) => new DnsMessageFlatPurposeDto
            {
                Id = t.t.t.message.Id,
                Timestamp = t.t.t.message.Timestamp,
                TransactionId = t.t.t.message.TransactionId,
                QueryName = t.t.t.message.QueryName,
                QueryType = t.t.t.message.QueryType,
                IsQuery = t.t.t.message.IsQuery,
                ResponseAddresses = t.t.t.message.ResponseAddresses,
                FileAnalysisId = t.t.t.message.FileAnalysisId,

                SenderId = t.t.t.message.SenderId,
                SenderAddress = t.t.t.message.Sender != null ? t.t.t.message.Sender.Address : null,
                SenderPort = t.t.t.message.Sender != null ? t.t.t.message.Sender.Port : null,

                RecipientId = t.t.t.message.RecipientId,
                RecipientAddress = t.t.t.message.Recipient != null ? t.t.t.message.Recipient.Address : null,
                RecipientPort = t.t.t.message.Recipient != null ? t.t.t.message.Recipient.Port : null,

                Purpose = knownDomain == null ? null : knownDomain.Purpose
            });

        return await query.ToListAsync();
    }
}
