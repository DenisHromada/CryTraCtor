namespace CryTraCtor.Database.Entities;

public class FileAnalysisEntity : IEntity
{
    public required Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid StoredFileId { get; set; }
    public StoredFileEntity? StoredFile { get; set; }

    public virtual ICollection<TrafficParticipantEntity>? TrafficParticipants { get; set; }
    public virtual ICollection<DnsPacketEntity>? DnsPackets { get; set; }
}
