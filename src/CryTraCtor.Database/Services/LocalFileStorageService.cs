using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CryTraCtor.Database.Services;

public class LocalFileStorageService(
    IConfiguration configuration
) : IFileStorageService
{
    public string CaptureFileDirectory { get; set; } = configuration["FileStorageDirectory"] ?? string.Empty;

    public async Task<string> StoreFileAsync(Stream incomingStream)
    {
        var internalPath = GenerateInternalFilePath();

        await using var storageStream = File.Create(internalPath);
        await incomingStream.CopyToAsync(storageStream);

        return internalPath;
    }

    private string GenerateInternalFilePath()
    {
        var localFileStorageDirectory = CaptureFileDirectory;
        var internalFileName = Path.GetRandomFileName();
        return Path.Combine(localFileStorageDirectory, internalFileName);
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            throw new ArgumentException("File not found");
        }
    }
}