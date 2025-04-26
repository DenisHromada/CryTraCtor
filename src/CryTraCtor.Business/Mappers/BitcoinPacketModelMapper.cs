using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers;

public class BitcoinPacketModelMapper(TrafficParticipantListModelMapper trafficParticipantMapper)
    : ModelMapperBase<BitcoinPacketEntity, BitcoinPacketListModel, BitcoinPacketDetailModel>
{
    public override BitcoinPacketEntity MapToEntity(BitcoinPacketDetailModel model)
        => new()
        {
            Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
            FileAnalysisId = model.FileAnalysisId,
            SenderId = model.SenderId,
            RecipientId = model.RecipientId,
            Timestamp = model.Timestamp
        };

    public override BitcoinPacketListModel MapToListModel(BitcoinPacketEntity? entity)
    {
        if (entity == null)
        {
            return null!;
        }

        return new BitcoinPacketListModel
        {
            Id = entity.Id,
            FileAnalysisId = entity.FileAnalysisId,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Timestamp = entity.Timestamp
        };
    }

    public override BitcoinPacketDetailModel? MapToDetailModel(BitcoinPacketEntity? entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new BitcoinPacketDetailModel
        {
            Id = entity.Id,
            FileAnalysisId = entity.FileAnalysisId,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Timestamp = entity.Timestamp,
            Sender = trafficParticipantMapper.MapToListModel(entity.Sender),
            Recipient = trafficParticipantMapper.MapToListModel(entity.Recipient)
        };
    }
}
