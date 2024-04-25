using CryTraCtor.Business.Models.KnownDomain;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IKnownDomainImportFacade
{
    public Task Create(IEnumerable<KnownDomainImportModel> modelCollection);
}