namespace CryTraCtor.APi.Services;

public interface IFileStorageService
{
    public string CaptureFileDirectory { get; set; }
    public void StoreFile(IFormFile file);
    public Task StoreFileAsync(IFormFile file);
}