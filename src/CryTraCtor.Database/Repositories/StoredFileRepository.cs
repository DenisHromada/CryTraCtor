using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Services;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class StoredFileRepository(
    IDbContextFactory<CryTraCtorDbContext> dbContextFactory,
    IEntityMapper<StoredFileEntity> entityMapper,
    IFileStorageService fileStorageService
) : IStoredFileRepository
{
    public IQueryable<StoredFileEntity> GetMetadataAll() => dbContextFactory.CreateDbContext().Set<StoredFileEntity>();

    public async Task<StoredFileEntity?> GetMetadataByFilenameAsync(string filename)
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

        try
        {
            fileStorageService.DeleteFile(file.InternalFilePath);
        }
        catch (ArgumentException e)
        {
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

    public async Task<StoredFileEntity> InsertAsync(StoredFileEntity entity, Stream incomingStream)
    {
        if (entity.FileSize <= 0)
        {
            throw new ArgumentException("File is empty");
        }

        try
        {
            entity.InternalFilePath = await fileStorageService.StoreFileAsync(incomingStream);

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            if (await ExistsAsync(entity))
            {
                throw new ArgumentException("File already exists");
            }

            await dbContext.StoredFile.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            fileStorageService.DeleteFile(entity.InternalFilePath);
            throw new Exception("Failed to store file", e);
        }
    }

    public async Task<StoredFileEntity> RenameAsync(string oldName, string newName)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingEntity = await GetMetadataByFilenameAsync(oldName);

        if (existingEntity is null)
        {
            throw new ArgumentException("File not found");
        }

        existingEntity.PublicFileName = newName;

        dbContext.StoredFile.Update(existingEntity);
        await dbContext.SaveChangesAsync();
        return existingEntity;
    }
    // Unsafe to expose to users. Internal path not sanitized.
    public async Task<StoredFileEntity> UpdateAsync(StoredFileEntity entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingEntity = await GetMetadataByFilenameAsync(entity.PublicFileName);
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
