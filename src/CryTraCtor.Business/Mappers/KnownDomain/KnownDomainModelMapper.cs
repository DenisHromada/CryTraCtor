using CryTraCtor.Business.Mappers.CryptoProduct;
using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.KnownDomain;

public class KnownDomainModelMapper(
    KnownDomainListModelMapper knownDomainListModelMapper,
    CryptoProductListModelMapper cryptoProductListModelMapper
) : ModelMapperBase<KnownDomainEntity, KnownDomainListModel, KnownDomainDetailModel>

{
    public override KnownDomainListModel MapToListModel(KnownDomainEntity? entity)
        => knownDomainListModelMapper.MapToListModel(entity);

    public override KnownDomainDetailModel MapToDetailModel(KnownDomainEntity? entity)
        => entity is null
            ? KnownDomainDetailModel.Empty()
            : new KnownDomainDetailModel
            {
                Id = entity.Id,
                DomainName = entity.DomainName,
                Purpose = entity.Purpose,
                Description = entity.Description,
                CryptoProduct = cryptoProductListModelMapper.MapToListModel(entity.CryptoProduct)
            };

    public IEnumerable<KnownDomainDetailModel> MapToDetailModel(IEnumerable<KnownDomainEntity> entities)
        => entities.Select(MapToDetailModel);

    public override KnownDomainEntity MapToEntity(KnownDomainDetailModel model)
        => new()
        {
            Id = model.Id,
            DomainName = model.DomainName,
            Purpose = model.Purpose,
            Description = model.Description,
            CryptoProductId = model.CryptoProduct.Id,
            CryptoProduct = null
        };
}
