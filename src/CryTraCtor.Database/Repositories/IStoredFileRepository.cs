using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public interface IStoredFileRepository
{
    IQueryable<StoredFileEntity> GetMetadataAll();
    Task<StoredFileEntity?> GetMetadataByFilenameAsync(string filename);
    Task DeleteAsync(string publicFileName);
    ValueTask<bool> ExistsAsync(StoredFileEntity entity);
    Task<StoredFileEntity> InsertAsync(StoredFileEntity entity, Stream incomingStream);
    Task<StoredFileEntity> UpdateAsync(StoredFileEntity entity);
}