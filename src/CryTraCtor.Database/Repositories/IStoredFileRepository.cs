using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public interface IStoredFileRepository
{
    Task<List<StoredFileEntity>> GetMetadataAllAsync();
    Task<StoredFileEntity?> GetMetadataByIdAsync(Guid id);
    Task<StoredFileEntity?> GetMetadataByFilenameAsync(string filename);
    Task DeleteAsync(string publicFileName);
    ValueTask<bool> ExistsAsync(StoredFileEntity entity);
    Task<StoredFileEntity> InsertAsync(StoredFileEntity entity, Stream incomingStream);
    Task<StoredFileEntity> UpdateAsync(StoredFileEntity entity);
    Task<StoredFileEntity> RenameAsync(string oldFileName, string newFileName);
}
