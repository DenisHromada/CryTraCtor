using CryTraCtor.Business.Mappers.KnownDomain;
using CryTraCtor.Business.Mappers.ModelMapperBase;
using CryTraCtor.Business.Models.CryptoProduct;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.CryptoProduct;

public class CryptoProductModelMapper(
    CryptoProductListModelMapper cryptoProductListModelMapper,
    KnownDomainListModelMapper knownDomainListModelMapper
) : ModelMapperBase<CryptoProductEntity, CryptoProductListModel, CryptoProductDetailModel>
{
    public override CryptoProductListModel MapToListModel(CryptoProductEntity? entity)
        => cryptoProductListModelMapper.MapToListModel(entity);

    public override CryptoProductDetailModel MapToDetailModel(CryptoProductEntity? entity) =>
        entity is null
            ? CryptoProductDetailModel.Empty()
            : new CryptoProductDetailModel
            {
                Id = entity.Id,
                Vendor = entity.Vendor,
                ProductName = entity.ProductName,
                KnownDomains = knownDomainListModelMapper.MapToListModel(entity.KnownDomains).ToList()
            };

    public override CryptoProductEntity MapToEntity(CryptoProductDetailModel model)
        => new()
        {
            Id = model.Id,
            Vendor = model.Vendor,
            ProductName = model.ProductName
        };
}