using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class StoredFileMapper : IEntityMapper<StoredFile>
{
    public void MapToExistingEntity(StoredFile existingEntity, StoredFile newEntity)
    {
        existingEntity.PublicFileName = newEntity.PublicFileName;
        existingEntity.InternalFilePath = newEntity.InternalFilePath;
        existingEntity.MimeType = newEntity.MimeType;
        existingEntity.FileSize = newEntity.FileSize;
    }
}