using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class CryptoProductEntityMapper : IEntityMapper<CryptoProductEntity>
{
    public void MapToExistingEntity(CryptoProductEntity existingEntity, CryptoProductEntity newEntity)
    {
        existingEntity.Vendor = newEntity.Vendor;
        existingEntity.ProductName = newEntity.ProductName;
    }
}