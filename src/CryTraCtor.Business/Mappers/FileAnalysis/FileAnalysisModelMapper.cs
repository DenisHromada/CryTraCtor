using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models.FileAnalysis;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.FileAnalysis;

public class FileAnalysisModelMapper(
    FileAnalysisListModelMapper fileAnalysisListModelMapper,
    TrafficParticipantListModelMapper trafficParticipantListModelMapper
) : ModelMapperBase<FileAnalysisEntity, FileAnalysisListModel, FileAnalysisDetailModel>
{
    public override FileAnalysisListModel MapToListModel(FileAnalysisEntity? entity)
        => fileAnalysisListModelMapper.MapToListModel(entity);

    public override FileAnalysisDetailModel MapToDetailModel(FileAnalysisEntity? entity)
        => entity is null
            ? FileAnalysisDetailModel.Empty()
            : new FileAnalysisDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt,
                StoredFileId = entity.StoredFileId,
                TrafficParticipants =
                    trafficParticipantListModelMapper
                        .MapToListModel(entity.TrafficParticipants ?? [])
                        .ToList()
            };

    public override FileAnalysisEntity MapToEntity(FileAnalysisDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            CreatedAt = model.CreatedAt,
            StoredFileId = model.StoredFileId
        };
}
