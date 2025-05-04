using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers;

public class DnsMessageModelMapper(
    TrafficParticipantListModelMapper trafficParticipantListModelMapper
) : ModelMapperBase<DnsPacketEntity, DnsMessageModel, DnsMessageModel>
{
    public override DnsPacketEntity MapToEntity(DnsMessageModel model)
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

    public override DnsMessageModel MapToListModel(DnsPacketEntity? entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new DnsMessageModel
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
                : trafficParticipantListModelMapper.MapToListModel(entity.Recipient),
            KnownDomainPurpose = null
        };
    }

    public override DnsMessageModel? MapToDetailModel(DnsPacketEntity? entity) => MapToListModel(entity);
}
