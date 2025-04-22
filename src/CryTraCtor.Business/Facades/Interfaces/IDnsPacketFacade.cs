using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IDnsPacketFacade : IFacade<DnsPacketEntity, DnsPacketModel, DnsPacketModel>
{
    Task<IEnumerable<DnsPacketModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
}
