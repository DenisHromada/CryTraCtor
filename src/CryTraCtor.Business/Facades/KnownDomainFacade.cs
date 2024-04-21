using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.KnownDomain;
using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class KnownDomainFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    KnownDomainModelMapper modelMapper
) : FacadeBase<
        KnownDomainEntity,
        KnownDomainListModel,
        KnownDomainDetailModel,
        KnownDomainEntityMapper>(unitOfWorkFactory, modelMapper),
    IKnownDomainFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[] {$"{nameof(KnownDomainEntity.CryptoProduct)}"};

    public async Task<IEnumerable<KnownDomainDetailModel>> GetAllDetailAsync()
    {
        var unitOfWork = UnitOfWorkFactory.Create();
        var query = unitOfWork
            .GetRepository<KnownDomainEntity, KnownDomainEntityMapper>()
            .Get();
        
        foreach (var includePath in IncludesNavigationPathDetail)
        {
            query = query.Include($"{nameof(KnownDomainEntity.CryptoProduct)}");
        }

        var a = await query
            .ToListAsync()
            .ConfigureAwait(false);
        return modelMapper.MapToDetailModel(a);
    }
}