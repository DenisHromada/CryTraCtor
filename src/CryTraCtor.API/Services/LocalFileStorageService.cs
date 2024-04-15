using CryTraCtor.Database;
using CryTraCtor.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.APi.Services;

public class LocalFileStorageService(
    IConfiguration configuration,
    IDbContextFactory<CryTraCtorDbContext> contextFactory
) : IFileStorageService
{
    public string CaptureFileDirectory { get; set; } = configuration["FileStorageDirectory"] ?? string.Empty;

    public void StoreFile(IFormFile file) => _ = StoreFileAsync(file);

    public async Task StoreFileAsync(IFormFile file)
    {
        var size = file.Length;
        if (size <= 0)
        {
            throw new ArgumentException("File is empty");
        }

        var fileDbEntity = new StoredFile
        {
            Id = new Guid(),
            PublicFileName = file.FileName,
            MimeType = file.ContentType,
            FileSize = size,
            InternalFilePath = GenerateInternalFilePath()
        };

        await using var stream = File.Create(fileDbEntity.InternalFilePath);
        await file.CopyToAsync(stream);
        
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        await dbContext.StoredFiles.AddAsync(fileDbEntity);
        await dbContext.SaveChangesAsync();
    }

    private string GenerateInternalFilePath()
    {
        var localFileStorageDirectory = CaptureFileDirectory;
        var internalFileName = Path.GetRandomFileName();
        return Path.Combine(localFileStorageDirectory, internalFileName);
    }
}