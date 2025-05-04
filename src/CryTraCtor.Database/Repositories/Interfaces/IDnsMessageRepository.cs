using CryTraCtor.Database.Dtos;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories.Interfaces;

public interface IDnsMessageRepository : IRepository<DnsMessageEntity>
{
    Task<IEnumerable<DnsMessageEntity>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
    Task<IEnumerable<DnsMessageFlatPurposeDto>> GetMessagesWithFlatPurposeByFileAnalysisIdAsync(Guid fileAnalysisId);
}
