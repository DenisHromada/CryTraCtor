namespace CryTraCtor.Database.Services;

public interface IFileStorageService
{
    public string FileStorageDirectory { get; set; }
    public Task<string> StoreFileAsync(Stream stream);
    public void DeleteFile(string fileName);
}