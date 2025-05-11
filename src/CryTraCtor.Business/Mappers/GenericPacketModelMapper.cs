using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers;

public class GenericPacketModelMapper : IModelMapper<GenericPacketEntity, GenericPacketModel, GenericPacketModel>
{
    public GenericPacketModel MapToListModel(GenericPacketEntity entity)
    {
        return MapToDetailModel(entity);
    }

    public IEnumerable<GenericPacketModel> MapToListModel(IEnumerable<GenericPacketEntity> entities)
    {
        return entities.Select(MapToListModel);
    }

    public GenericPacketModel MapToDetailModel(GenericPacketEntity entity)
    {
        return new GenericPacketModel
        {
            Id = entity.Id,
            FileAnalysisId = entity.FileAnalysisId,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Timestamp = entity.Timestamp
        };
    }

    public GenericPacketEntity MapToEntity(GenericPacketModel model)
    {
        return new GenericPacketEntity
        {
            Id = model.Id,
            FileAnalysisId = model.FileAnalysisId,
            SenderId = model.SenderId,
            RecipientId = model.RecipientId,
            Timestamp = model.Timestamp
        };
    }
}
