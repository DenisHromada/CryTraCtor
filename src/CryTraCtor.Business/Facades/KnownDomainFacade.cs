using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using CryTraCtor.Facades.Interfaces;
using CryTraCtor.Mappers.KnownDomain;
using CryTraCtor.Models.KnownDomain;

namespace CryTraCtor.Facades;

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
}
