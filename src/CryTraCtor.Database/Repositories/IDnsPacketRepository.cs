using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Dtos;

namespace CryTraCtor.Database.Repositories;

public interface IDnsPacketRepository : IRepository<DnsPacketEntity>
{
    Task<IEnumerable<DnsPacketEntity>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
    Task<IEnumerable<DnsPacketFlatPurposeDto>> GetPacketsWithFlatPurposeByFileAnalysisIdAsync(Guid fileAnalysisId);
}
