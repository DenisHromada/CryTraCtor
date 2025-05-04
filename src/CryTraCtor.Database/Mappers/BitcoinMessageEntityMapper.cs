using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class BitcoinMessageEntityMapper : IEntityMapper<BitcoinMessageEntity>
{
    public void MapToExistingEntity(BitcoinMessageEntity existingEntity, BitcoinMessageEntity newEntity)
    {
        existingEntity.FileAnalysisId = newEntity.FileAnalysisId;
        existingEntity.SenderId = newEntity.SenderId;
        existingEntity.RecipientId = newEntity.RecipientId;
        existingEntity.Timestamp = newEntity.Timestamp;
    }
}
