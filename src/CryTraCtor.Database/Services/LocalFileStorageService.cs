using Microsoft.Extensions.Configuration;

namespace CryTraCtor.Database.Services;

public class LocalFileStorageService : IFileStorageService
{
    public LocalFileStorageService(IConfiguration configuration)
    {
        var fileStorageDirectory = configuration["FileStorageDirectory"];
        
        if (string.IsNullOrWhiteSpace(fileStorageDirectory))
        {
            fileStorageDirectory = Environment.GetEnvironmentVariable("FILE_STORAGE_DIRECTORY");
        }
        
        if (string.IsNullOrWhiteSpace(fileStorageDirectory))
        {
            fileStorageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "StoredFiles");
        }

        FileStorageDirectory = fileStorageDirectory;
        if (!Directory.Exists(FileStorageDirectory))
        {
            Directory.CreateDirectory(FileStorageDirectory);
        }
    }
    

    public string FileStorageDirectory { get; set; }

    public async Task<string> StoreFileAsync(Stream incomingStream)
    {
        var internalPath = GenerateInternalFilePath();

        await using var storageStream = File.Create(internalPath);
        await incomingStream.CopyToAsync(storageStream);

        return internalPath;
    }

    private string GenerateInternalFilePath()
    {
        var localFileStorageDirectory = FileStorageDirectory;
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