using CryTraCtor.Database.Entities;
using Microsoft.AspNetCore.Http;

namespace CryTraCtor.Database.Repositories;

public interface IStoredFileRepository
{
    IQueryable<StoredFileEntity> GetMetadataAll();
    Task<StoredFileEntity?> GetMetadataByFilenameAsync(string filename);
    Task DeleteAsync(string publicFileName);
    ValueTask<bool> ExistsAsync(StoredFileEntity entity);
    Task<StoredFileEntity> InsertAsync(IFormFile entity);
    Task<StoredFileEntity> UpdateAsync(StoredFileEntity entity);
}