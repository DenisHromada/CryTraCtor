using CryTraCtor.Database.Entities;
using Microsoft.AspNetCore.Http;

namespace CryTraCtor.Database.Repositories;

public interface IStoredFileRepository
{
    IQueryable<StoredFileEntity> GetAll();
    Task<StoredFileEntity?> GetByFilenameAsync(string filename);
    Task DeleteAsync(string publicFileName);
    ValueTask<bool> ExistsAsync(StoredFileEntity entity);
    Task<StoredFileEntity> InsertAsync(IFormFile entity);
    Task<StoredFileEntity> UpdateAsync(StoredFileEntity entity);
}