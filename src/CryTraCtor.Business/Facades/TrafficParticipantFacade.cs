using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class TrafficParticipantFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    TrafficParticipantModelMapper modelMapper
) : FacadeBase<
        TrafficParticipantEntity,
        TrafficParticipantListModel,
        TrafficParticipantDetailModel,
        TrafficParticipantEntityMapper>(unitOfWorkFactory, modelMapper),
    ITrafficParticipantFacade
{
    public async Task<IEnumerable<TrafficParticipantListModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var participantRepository = uow.GetRepository<TrafficParticipantEntity, TrafficParticipantEntityMapper>();
        var dnsPacketRepository = uow.GetRepository<DnsPacketEntity, DnsPacketEntityMapper>();

        var participantsTask = participantRepository.Get()
            .Where(tp => tp.FileAnalysisId == fileAnalysisId)
            .ToListAsync();

        var dnsPacketsTask = dnsPacketRepository.Get()
            .Where(dns => dns.FileAnalysisId == fileAnalysisId)
            .Select(dns => new { dns.SenderId, dns.RecipientId }) // Select only needed IDs
            .ToListAsync();

        await Task.WhenAll(participantsTask, dnsPacketsTask);

        var participantEntities = await participantsTask;
        var dnsPacketIds = await dnsPacketsTask;

        var outgoingCounts = dnsPacketIds
            .GroupBy(dns => dns.SenderId)
            .ToDictionary(g => g.Key, g => g.Count());

        var incomingCounts = dnsPacketIds
            .GroupBy(dns => dns.RecipientId)
            .ToDictionary(g => g.Key, g => g.Count());

        var participantModels = participantEntities.Select(entity =>
        {
            var model = ModelMapper.MapToListModel(entity);
            model.OutgoingDnsCount = outgoingCounts.TryGetValue(entity.Id, out var outCount) ? outCount : 0;
            model.IncomingDnsCount = incomingCounts.TryGetValue(entity.Id, out var inCount) ? inCount : 0;
            return model;
        }).ToList();

        return participantModels;
    }
}
