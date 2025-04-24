using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.DomainMatch;
using CryTraCtor.Database.Repositories;
using CryTraCtor.Business.Mappers.DomainMatch;

namespace CryTraCtor.Business.Facades
{
    public class DomainMatchFacade(
        IDomainMatchRepository domainMatchRepository,
        DomainMatchMapper domainMatchMapper
    ) : IDomainMatchFacade
    {
        public async Task CreateMatchAsync(DomainMatchModel model)
        {
            var entity = domainMatchMapper.MapModelToEntity(model);
            await domainMatchRepository.InsertAsync(entity);
        }

        public async Task CreateMatchesAsync(IEnumerable<DomainMatchModel> models)
        {
            var entities = models.Select(domainMatchMapper.MapModelToEntity);
            await domainMatchRepository.InsertRangeAsync(entities);
        }
    }
}
