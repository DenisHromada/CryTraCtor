using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Mappers;

public class FileAnalysisEntityMapper : IEntityMapper<FileAnalysisEntity>
{
    public void MapToExistingEntity(FileAnalysisEntity existingEntity, FileAnalysisEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.StoredFileId = newEntity.StoredFileId;
    }
} 