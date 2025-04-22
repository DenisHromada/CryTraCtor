using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class DnsPacketRepository(CryTraCtorDbContext dbContext, DnsPacketEntityMapper entityMapper)
    : Repository<DnsPacketEntity>(dbContext, entityMapper), IDnsPacketRepository
{
    public async Task<IEnumerable<DnsPacketEntity>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        return await Get()
            .Include(e => e.Sender)
            .Include(e => e.Recipient)
            .Where(e => e.FileAnalysisId == fileAnalysisId)
            .ToListAsync();
    }
}
