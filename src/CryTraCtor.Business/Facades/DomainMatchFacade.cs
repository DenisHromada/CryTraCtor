using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.DomainMatch;
using CryTraCtor.Business.Mappers.DomainMatch;
using CryTraCtor.Database.UnitOfWork;


namespace CryTraCtor.Business.Facades;

public class DomainMatchFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    DomainMatchMapper domainMatchMapper
) : IDomainMatchFacade
{
    public async Task CreateMatchAsync(DomainMatchModel model)
    {
        var entity = domainMatchMapper.MapModelToEntity(model);
        await using var uow = unitOfWorkFactory.Create();
        var domainMatchRepository = uow.DomainMatches;
        await domainMatchRepository.InsertAsync(entity);
        await uow.CommitAsync();
    }

    public async Task CreateMatchesAsync(IEnumerable<DomainMatchModel> models)
    {
        var entities = models.Select(domainMatchMapper.MapModelToEntity);
        if (!entities.Any()) return;

        await using var uow = unitOfWorkFactory.Create();
        var domainMatchRepository = uow.DomainMatches;
        await domainMatchRepository.InsertRangeAsync(entities);
        await uow.CommitAsync();
    }
}
