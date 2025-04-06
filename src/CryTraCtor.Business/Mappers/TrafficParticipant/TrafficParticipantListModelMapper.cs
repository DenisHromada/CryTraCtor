using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.TrafficParticipant;

public class
    TrafficParticipantListModelMapper : ListModelMapperBase<TrafficParticipantEntity, TrafficParticipantListModel>
{
    public override TrafficParticipantListModel MapToListModel(TrafficParticipantEntity? entity)
        => entity is null
            ? TrafficParticipantListModel.Empty()
            : new TrafficParticipantListModel
            {
                Id = entity.Id, Address = entity.Address, Port = entity.Port, FileAnalysisId = entity.FileAnalysisId
            };
}
