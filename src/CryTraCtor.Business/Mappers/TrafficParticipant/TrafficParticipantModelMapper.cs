using CryTraCtor.Business.Mappers.ModelMapperBase;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.TrafficParticipant;

public class TrafficParticipantModelMapper(
    TrafficParticipantListModelMapper trafficParticipantListModelMapper,
    StoredFileModelMapper storedFileModelMapper
) : ModelMapperBase<TrafficParticipantEntity, TrafficParticipantListModel, TrafficParticipantDetailModel>
{
    public override TrafficParticipantListModel MapToListModel(TrafficParticipantEntity? entity)
        => trafficParticipantListModelMapper.MapToListModel(entity);

    public override TrafficParticipantDetailModel MapToDetailModel(TrafficParticipantEntity? entity)
        => entity is null
            ? TrafficParticipantDetailModel.Empty()
            : new TrafficParticipantDetailModel
            {
                Id = entity.Id,
                Address = entity.Address,
                Port = entity.Port,
                StoredFile = storedFileModelMapper.MapToListModel(entity.StoredFile)
            };

    public override TrafficParticipantEntity MapToEntity(TrafficParticipantDetailModel model)
        => new()
        {
            Id = model.Id,
            Address = model.Address,
            Port = model.Port,
            StoredFileId = model.StoredFile.Id,
            StoredFile = null
        };
}
