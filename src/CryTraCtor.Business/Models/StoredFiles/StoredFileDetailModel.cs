namespace CryTraCtor.Business.Models.StoredFiles;

public record StoredFileDetailModel : IModel
{
    public required Guid Id { get; set; }
    public required string PublicFileName { get; set; }
    public required string InternalFilePath { get; set; }
    public required string MimeType { get; set; }
    public required long FileSize { get; set; }

    public static StoredFileDetailModel Empty()
        => new()
        {
            Id = Guid.Empty,
            PublicFileName = string.Empty,
            InternalFilePath = string.Empty,
            MimeType = string.Empty,
            FileSize = 0
        };
}
