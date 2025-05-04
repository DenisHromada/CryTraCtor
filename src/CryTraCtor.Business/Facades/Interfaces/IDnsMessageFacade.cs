using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IDnsMessageFacade : IFacade<DnsMessageEntity, DnsMessageModel, DnsMessageModel>
{
    Task<IEnumerable<DnsMessageModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
}
