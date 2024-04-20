using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.KnownDomain;
using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;

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
}
