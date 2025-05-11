using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;
using CryTraCtor.Business.Models.Aggregates;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IKnownDomainFacade : IFacade<KnownDomainEntity, KnownDomainListModel, KnownDomainDetailModel>
{
    public Task<IEnumerable<KnownDomainDetailModel>> GetAllDetailAsync();
    public Task<IEnumerable<KnownServiceDataModel>> GetKnownServicesDataAsync(Guid fileAnalysisId);
}
