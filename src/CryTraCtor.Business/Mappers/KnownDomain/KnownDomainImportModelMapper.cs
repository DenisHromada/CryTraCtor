using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.KnownDomain;

public class KnownDomainImportModelMapper
{
    public (CryptoProductEntity, KnownDomainEntity) MapToEntity(KnownDomainImportModel model) =>
        new(
            new CryptoProductEntity
            {
                Id = Guid.Empty,
                Vendor = model.Vendor,
                ProductName = model.ProductName
            },
            new KnownDomainEntity
            {
                Id = Guid.Empty,
                DomainName = model.DomainName,
                Purpose = model.Purpose,
                Description = model.Description
            });
}