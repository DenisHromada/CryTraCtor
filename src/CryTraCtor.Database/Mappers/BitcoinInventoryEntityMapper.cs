using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class BitcoinInventoryEntityMapper : IEntityMapper<BitcoinInventoryEntity>
{
    public void MapToExistingEntity(BitcoinInventoryEntity existingEntity, BitcoinInventoryEntity sourceEntity)
    {
        existingEntity.Type = sourceEntity.Type;
        existingEntity.Hash = sourceEntity.Hash;
    }
}
