using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(PublicFileName), IsUnique = true)]
public class StoredFileEntity : IEntity
{
    public required Guid Id { get; set; }

    public string PublicFileName { get; set; } = null!;
    public string InternalFilePath { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public long FileSize { get; set; }
}