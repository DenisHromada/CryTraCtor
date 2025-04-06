using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Models.FileAnalysis;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.FileAnalysis;

public class FileAnalysisListModelMapper : ListModelMapperBase<FileAnalysisEntity, FileAnalysisListModel>
{
    public override FileAnalysisListModel MapToListModel(FileAnalysisEntity? entity)
        => entity is null
            ? FileAnalysisListModel.Empty()
            : new FileAnalysisListModel
            {
                Id = entity.Id, Name = entity.Name, CreatedAt = entity.CreatedAt, StoredFileId = entity.StoredFileId
            };
}
