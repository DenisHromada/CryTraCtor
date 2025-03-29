namespace CryTraCtor.Business.Models.StoredFiles;

public class StoredFileCreateModel(string publicFileName, string mimeType, long fileSize)
{
    public string PublicFileName { get; set; } = publicFileName;
    public string MimeType { get; set; } = mimeType;
    public long FileSize { get; set; } = fileSize;
}
