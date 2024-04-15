namespace CryTraCtor.Database.Services;

public interface IFileStorageService
{
    public string CaptureFileDirectory { get; set; }
    public void StoreFile(IFormFile file);
    public Task StoreFileAsync(IFormFile file);
    public void DeleteFile(string fileName);
    public Task DeleteFileAsync(string fileName);
}