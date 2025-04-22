using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public interface IDnsPacketRepository : IRepository<DnsPacketEntity>
{
    Task<IEnumerable<DnsPacketEntity>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
}
