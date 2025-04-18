using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.KnownDomain;

public class KnownDomainImportModelMapper
{
    public (CryptoProductEntity, KnownDomainEntity) MapToEntity(KnownDomainImportModel model)
    {
        var cryptoProduct = new CryptoProductEntity
        {
            Id = Guid.NewGuid(),
            Vendor = model.Vendor,
            ProductName = model.ProductName
        };

        var knownDomain = new KnownDomainEntity
        {
            Id = Guid.NewGuid(),
            DomainName = model.DomainName,
            Purpose = model.Purpose,
            Description = model.Description,
            CryptoProductId = cryptoProduct.Id
        };

        return (cryptoProduct, knownDomain);
    }
}
