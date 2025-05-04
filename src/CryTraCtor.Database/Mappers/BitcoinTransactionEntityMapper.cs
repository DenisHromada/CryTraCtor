using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class BitcoinTransactionEntityMapper : IEntityMapper<BitcoinTransactionEntity>
{
    public BitcoinTransactionEntity Map(BitcoinTransactionEntity entity)
    {
        return entity;
    }

    public void MapToExistingEntity(BitcoinTransactionEntity existingEntity, BitcoinTransactionEntity newEntity)
    {
    }
}
