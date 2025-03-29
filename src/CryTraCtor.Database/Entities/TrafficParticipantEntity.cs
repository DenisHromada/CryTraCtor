using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(Address), nameof(Port), nameof(StoredFileId), IsUnique = true)]
public class TrafficParticipantEntity : IEntity
{
    public required Guid Id { get; set; }

    public string Address { get; set; } = null!;
    public int Port { get; set; }

    public Guid StoredFileId { get; set; }
    public StoredFileEntity? StoredFile { get; set; }
}
