using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers;

public class DnsMessageModelMapper(
    TrafficParticipantListModelMapper trafficParticipantListModelMapper
) : ModelMapperBase<DnsMessageEntity, DnsMessageModel, DnsMessageModel>
{
    public override DnsMessageEntity MapToEntity(DnsMessageModel model)
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
            FileAnalysisId = model.FileAnalysisId,
            ResolvedTrafficParticipants = model.ResolvedTrafficParticipants.Select(p =>
                new DnsMessageResolvedTrafficParticipantEntity
                {
                    DnsMessageId = model.Id,
                    TrafficParticipantId = p.Id
                }).ToList()
        };

    public override DnsMessageModel MapToListModel(DnsMessageEntity entity)
    {
        var model = new DnsMessageModel
        {
            Id = entity.Id,
            SenderId = entity.SenderId,
            RecipientId = entity.RecipientId,
            Timestamp = entity.Timestamp,
            TransactionId = entity.TransactionId,
            QueryName = entity.QueryName,
            QueryType = entity.QueryType,
            IsQuery = entity.IsQuery,
            FileAnalysisId = entity.FileAnalysisId,
            Sender = entity.Sender == null ? null : trafficParticipantListModelMapper.MapToListModel(entity.Sender),
            Recipient = entity.Recipient == null
                ? null
                : trafficParticipantListModelMapper.MapToListModel(entity.Recipient),
            KnownDomainPurpose = null
        };

        if (entity.ResolvedTrafficParticipants != null)
        {
            model.ResolvedTrafficParticipants = entity.ResolvedTrafficParticipants
                .Where(rtp => rtp.TrafficParticipant != null)
                .Select(rtp => trafficParticipantListModelMapper.MapToListModel(rtp.TrafficParticipant!))
                .ToList();
        }
        else
        {
            model.ResolvedTrafficParticipants = [];
        }

        return model;
    }

    public override DnsMessageModel MapToDetailModel(DnsMessageEntity entity) => MapToListModel(entity);
}
