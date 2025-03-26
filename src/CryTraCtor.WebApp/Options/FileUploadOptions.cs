namespace CryTraCtor.WebApp.Options;

public sealed class FileUploadOptions
{
    public int MaxFileSizeInMb { get; set; }
    public int MaxFileSizeInBytes => MaxFileSizeInMb * 1024 * 1024;
}
