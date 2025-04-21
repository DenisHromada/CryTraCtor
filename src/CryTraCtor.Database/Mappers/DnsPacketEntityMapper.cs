using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class DnsPacketEntityMapper : IEntityMapper<DnsPacketEntity>
{
    public void MapToExistingEntity(DnsPacketEntity existingEntity, DnsPacketEntity newEntity)
    {
        // Currently assuming DnsPacketEntity is immutable after creation.
    }
}
