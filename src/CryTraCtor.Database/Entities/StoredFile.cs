using System.ComponentModel.DataAnnotations;

namespace CryTraCtor.Database.Entities;

public partial class StoredFile : IEntity
{
    [Key] public required Guid Id { get; set; }

    public string PublicFileName { get; set; } = null!;
    public string InternalFilePath { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public long FileSize { get; set; }
}