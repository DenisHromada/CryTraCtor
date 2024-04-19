using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class StoredFileRepository(
    IDbContextFactory<CryTraCtorDbContext> dbContextFactory,
    IEntityMapper<StoredFileEntity> entityMapper,
    IFileStorageService fileStorageService
) : IStoredFileRepository
{
    public IQueryable<StoredFileEntity> GetAll() => dbContextFactory.CreateDbContext().Set<StoredFileEntity>();
    
    public async Task<StoredFileEntity?> GetByFilenameAsync(string filename)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.StoredFile.FirstOrDefaultAsync(f => f.PublicFileName == filename);
    }
    public async Task DeleteAsync(string publicFileName)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var file = await dbContext.StoredFile
            .FirstOrDefaultAsync(f => f.PublicFileName == publicFileName);

        if (file is null)
        {
            throw new ArgumentException("File not found");
        }

        var filePath = file.InternalFilePath;

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        dbContext.StoredFile.Remove(file);
        await dbContext.SaveChangesAsync();
    }

    public async ValueTask<bool> ExistsAsync(StoredFileEntity entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var file = await dbContext.StoredFile.FirstOrDefaultAsync(
            f => f.PublicFileName == entity.PublicFileName
        );
        return file is not null;
    }

    public async Task<StoredFileEntity> InsertAsync(IFormFile file)
    {
        var size = file.Length;
        if (size <= 0)
        {
            throw new ArgumentException("File is empty");
        }

        var internalPath = await fileStorageService.StoreFileAsync(file);

        try
        {
            var fileDbEntity = new StoredFileEntity
            {
                Id = new Guid(),
                PublicFileName = file.FileName,
                MimeType = file.ContentType,
                FileSize = size,
                InternalFilePath = internalPath
            };

            await using var stream = File.Create(fileDbEntity.InternalFilePath);
            await file.CopyToAsync(stream);
            
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            await dbContext.StoredFile.AddAsync(fileDbEntity);
            await dbContext.SaveChangesAsync();
            return fileDbEntity;
        }
        catch (Exception e)
        {
            fileStorageService.DeleteFile(internalPath);
            throw new Exception("Failed to store file", e);
        }
    }

    public async Task<StoredFileEntity> UpdateAsync(StoredFileEntity entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingEntity = await dbContext.StoredFile
            .FirstOrDefaultAsync(f => f.PublicFileName == entity.PublicFileName);
        if (existingEntity is null)
        {
            throw new ArgumentException("File not found");
        }
        
        entityMapper.MapToExistingEntity(existingEntity, entity);
        dbContext.StoredFile.Update(existingEntity);
        await dbContext.SaveChangesAsync();
        return existingEntity;
    }
}