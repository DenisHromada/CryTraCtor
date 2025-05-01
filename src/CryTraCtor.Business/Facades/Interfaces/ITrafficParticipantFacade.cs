using CryTraCtor.Business.Models.Agregates;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface ITrafficParticipantFacade : IFacade<TrafficParticipantEntity, TrafficParticipantListModel, TrafficParticipantDetailModel>
{
    Task<IEnumerable<TrafficParticipantListModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId);
    Task<TrafficParticipantDetailModel?> GetByAddressPortAndFileAnalysisIdAsync(Guid fileAnalysisId, string address, int port);
    Task<TrafficParticipantKnownDomainSummaryModel?> GetKnownDomainSummaryAsync(Guid trafficParticipantId);
}
