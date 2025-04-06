using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class TrafficParticipantEntityMapper : IEntityMapper<TrafficParticipantEntity>
{
    public void MapToExistingEntity(TrafficParticipantEntity existingEntity, TrafficParticipantEntity newEntity)
    {
        existingEntity.Address = newEntity.Address;
        existingEntity.Port = newEntity.Port;
        existingEntity.FileAnalysisId = newEntity.FileAnalysisId;
    }
} 