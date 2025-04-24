using CryTraCtor.Business.Models.DomainMatch;

namespace CryTraCtor.Business.Facades.Interfaces
{
    public interface IDomainMatchFacade
    {
        Task CreateMatchAsync(DomainMatchModel model);
        Task CreateMatchesAsync(IEnumerable<DomainMatchModel> models);
    }
}
