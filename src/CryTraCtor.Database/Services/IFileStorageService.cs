using CryTraCtor.Database.Entities;
using Microsoft.AspNetCore.Http;

namespace CryTraCtor.Database.Services;

public interface IFileStorageService
{
    public string CaptureFileDirectory { get; set; }
    public Task<string> StoreFileAsync(IFormFile file);
    public void DeleteFile(string fileName);
}