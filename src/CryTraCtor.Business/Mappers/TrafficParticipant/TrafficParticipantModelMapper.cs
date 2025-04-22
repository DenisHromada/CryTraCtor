using CryTraCtor.Business.Mappers.FileAnalysis;
using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.TrafficParticipant;

public class TrafficParticipantModelMapper(
    TrafficParticipantListModelMapper trafficParticipantListModelMapper,
    FileAnalysisModelMapper fileAnalysisModelMapper,
    DnsPacketModelMapper dnsPacketModelMapper
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
                FileAnalysis = fileAnalysisModelMapper.MapToListModel(entity.FileAnalysis),
                SentDnsPackets = entity.SentDnsPackets?
                    .Select(dnsPacketModelMapper.MapToListModel).ToList() ?? [],
                ReceivedDnsPackets = entity.ReceivedDnsPackets?
                    .Select(dnsPacketModelMapper.MapToListModel).ToList() ?? []
            };

    public override TrafficParticipantEntity MapToEntity(TrafficParticipantDetailModel model)
        => new()
        {
            Id = model.Id,
            Address = model.Address,
            Port = model.Port,
            FileAnalysisId = model.FileAnalysis.Id,
            FileAnalysis = null,
            SentDnsPackets = null,
            ReceivedDnsPackets = null
        };
}
