using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class BitcoinPacketEntityMapper : IEntityMapper<BitcoinPacketEntity>
{
    public void MapToExistingEntity(BitcoinPacketEntity existingEntity, BitcoinPacketEntity newEntity)
    {
        existingEntity.FileAnalysisId = newEntity.FileAnalysisId;
        existingEntity.SenderId = newEntity.SenderId;
        existingEntity.RecipientId = newEntity.RecipientId;
        existingEntity.Timestamp = newEntity.Timestamp;
    }
}
