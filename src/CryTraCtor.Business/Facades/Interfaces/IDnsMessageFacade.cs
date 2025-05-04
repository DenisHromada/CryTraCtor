using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IDnsMessageFacade : IFacade<DnsPacketEntity, DnsMessageModel, DnsMessageModel>
{
    Task<IEnumerable<DnsMessageModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
}
