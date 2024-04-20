using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.CryptoProduct;
using CryTraCtor.Business.Models.CryptoProduct;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;

namespace CryTraCtor.Business.Facades;

public class CryptoProductFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    CryptoProductModelMapper modelMapper
) : FacadeBase<
        CryptoProductEntity,
        CryptoProductListModel,
        CryptoProductDetailModel,
        CryptoProductEntityMapper>(unitOfWorkFactory, modelMapper),
    ICryptoProductFacade
{
}