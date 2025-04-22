using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories;
using CryTraCtor.Database.UnitOfWork;


namespace CryTraCtor.Business.Facades;

public class TrafficParticipantFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    TrafficParticipantModelMapper modelMapper,
    TrafficParticipantAggregateRepository aggregateRepository
) : FacadeBase<
        TrafficParticipantEntity,
        TrafficParticipantListModel,
        TrafficParticipantDetailModel,
        TrafficParticipantEntityMapper>(unitOfWorkFactory, modelMapper),
    ITrafficParticipantFacade
{
    public async Task<IEnumerable<TrafficParticipantListModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        var aggregateDtos = await aggregateRepository.GetAggregatedByFileAnalysisIdAsync(fileAnalysisId);

        var participantModels = aggregateDtos.Select(dto => new TrafficParticipantListModel
        {
            Id = dto.Id,
            Address = dto.Address,
            Port = dto.Port,
            FileAnalysisId = dto.FileAnalysisId,
            OutgoingDnsCount = dto.OutgoingDnsCount,
            IncomingDnsCount = dto.IncomingDnsCount
        }).ToList();

        return participantModels;
    }
}
