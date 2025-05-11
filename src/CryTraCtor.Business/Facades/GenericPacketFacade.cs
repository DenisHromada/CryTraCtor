using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;

namespace CryTraCtor.Business.Facades;

public class GenericPacketFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    GenericPacketModelMapper modelMapper
) : FacadeBase<
        GenericPacketEntity,
        GenericPacketModel,
        GenericPacketModel,
        GenericPacketEntityMapper>(unitOfWorkFactory, modelMapper),
    IGenericPacketFacade;
