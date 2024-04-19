using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using CryTraCtor.Facades.Interfaces;
using CryTraCtor.Mappers.CryptoProduct;
using CryTraCtor.Models.CryptoProduct;

namespace CryTraCtor.Facades;

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