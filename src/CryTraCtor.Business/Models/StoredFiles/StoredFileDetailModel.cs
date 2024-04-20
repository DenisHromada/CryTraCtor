namespace CryTraCtor.Business.Models.StoredFiles;

public record StoredFileDetailModel : IModel
{
    public Guid Id { get; set; }
    public string PublicFileName { get; set; } = null!;
    public string InternalFilePath { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public long FileSize { get; set; }
    
    public static StoredFileDetailModel Empty()
        => new StoredFileDetailModel
        {
            Id = Guid.Empty,
            PublicFileName = string.Empty,
            InternalFilePath = string.Empty,
            MimeType = string.Empty,
            FileSize = 0
        };
}