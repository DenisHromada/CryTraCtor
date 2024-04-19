using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class StoredFileEntityMapper : IEntityMapper<StoredFileEntity>
{
    public void MapToExistingEntity(StoredFileEntity existingEntity, StoredFileEntity newEntity)
    {
        existingEntity.PublicFileName = newEntity.PublicFileName;
        existingEntity.InternalFilePath = newEntity.InternalFilePath;
        existingEntity.MimeType = newEntity.MimeType;
        existingEntity.FileSize = newEntity.FileSize;
    }
}