using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;

namespace CryTraCtor.Business.Facades;

public class DnsPacketFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    DnsPacketModelMapper modelMapper)
    : FacadeBase<DnsPacketEntity, DnsPacketModel, DnsPacketModel, DnsPacketEntityMapper>(unitOfWorkFactory,
        modelMapper), IDnsPacketFacade;
