using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers;

public class DnsPacketModelMapper(
    TrafficParticipantListModelMapper trafficParticipantListModelMapper
) : ModelMapperBase<DnsPacketEntity, DnsPacketModel, DnsPacketModel>
{
    public override DnsPacketEntity MapToEntity(DnsPacketModel model)
        => new()
        {
            Id = model.Id,
            SenderId = model.SenderId,
            RecipientId = model.RecipientId,
            Timestamp = model.Timestamp,
            TransactionId = model.TransactionId,
            QueryName = model.QueryName,
            QueryType = model.QueryType,
            IsQuery = model.IsQuery,
            ResponseAddresses = model.ResponseAddresses,
            FileAnalysisId = model.FileAnalysisId,
        };

    public override DnsPacketModel MapToListModel(DnsPacketEntity entity)
        => new()
        {
            Id = entity.Id,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Timestamp = entity.Timestamp,
            TransactionId = entity.TransactionId,
            QueryName = entity.QueryName,
            QueryType = entity.QueryType,
            IsQuery = entity.IsQuery,
            ResponseAddresses = entity.ResponseAddresses,
            FileAnalysisId = entity.FileAnalysisId,
            Sender = entity.Sender == null ? null : trafficParticipantListModelMapper.MapToListModel(entity.Sender),
            Recipient = entity.Recipient == null
                ? null
                : trafficParticipantListModelMapper.MapToListModel(entity.Recipient)
        };

    public override DnsPacketModel MapToDetailModel(DnsPacketEntity entity)
        => new()
        {
            Id = entity.Id,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Sender = entity.Sender == null ? null : trafficParticipantListModelMapper.MapToListModel(entity.Sender),
            Recipient = entity.Recipient == null
                ? null
                : trafficParticipantListModelMapper.MapToListModel(entity.Recipient),
            Timestamp = entity.Timestamp,
            TransactionId = entity.TransactionId,
            QueryName = entity.QueryName,
            QueryType = entity.QueryType,
            IsQuery = entity.IsQuery,
            ResponseAddresses = entity.ResponseAddresses,
            FileAnalysisId = entity.FileAnalysisId
        };
}
