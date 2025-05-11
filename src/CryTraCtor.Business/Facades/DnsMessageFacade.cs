using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using CryTraCtor.Business.Models.TrafficParticipants;

namespace CryTraCtor.Business.Facades;

public class DnsMessageFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    DnsMessageModelMapper modelMapper
)
    : FacadeBase<DnsMessageEntity, DnsMessageModel, DnsMessageModel, DnsMessageEntityMapper>(unitOfWorkFactory,
        modelMapper), IDnsMessageFacade
{
    public async Task<IEnumerable<DnsMessageModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        await using var uow = UnitOfWorkFactory.Create();

        var dnsPacketRepository = uow.DnsMessages;

        var flatPacketDtos = await dnsPacketRepository.GetMessagesWithFlatPurposeByFileAnalysisIdAsync(fileAnalysisId);

        if (!flatPacketDtos.Any())
        {
            return [];
        }

        var groupedPackets = flatPacketDtos
            .GroupBy(dto => dto.Id)
            .Select(group =>
            {
                var firstDto = group.First();

                var distinctPurposes = group
                    .Select(dto => dto.Purpose)
                    .Where(p => !string.IsNullOrEmpty(p))
                    .Distinct()
                    .ToList();

                var aggregatedPurpose = distinctPurposes.Any() ? string.Join(", ", distinctPurposes) : null;

                var senderModel = firstDto.SenderAddress != null && firstDto.SenderPort != null
                    ? new TrafficParticipantListModel
                        { Id = firstDto.SenderId, Address = firstDto.SenderAddress, Port = firstDto.SenderPort.Value }
                    : null;

                var recipientModel = firstDto.RecipientAddress != null && firstDto.RecipientPort != null
                    ? new TrafficParticipantListModel
                    {
                        Id = firstDto.RecipientId, Address = firstDto.RecipientAddress,
                        Port = firstDto.RecipientPort.Value
                    }
                    : null;

                return new DnsMessageModel
                {
                    Id = firstDto.Id,
                    Timestamp = firstDto.Timestamp,
                    TransactionId = firstDto.TransactionId,
                    QueryName = firstDto.QueryName,
                    QueryType = firstDto.QueryType,
                    IsQuery = firstDto.IsQuery,
                    FileAnalysisId = firstDto.FileAnalysisId,
                    SenderId = firstDto.SenderId,
                    RecipientId = firstDto.RecipientId,
                    Sender = senderModel,
                    Recipient = recipientModel,
                    KnownDomainPurpose = aggregatedPurpose
                };
            })
            .ToList();

        if (groupedPackets.Count != 0)
        {
            var messageIds = groupedPackets.Select(p => p.Id).Distinct().ToList();
            var resolvedParticipantDtos = await dnsPacketRepository.GetResolvedParticipantsForMessagesAsync(messageIds);

            var resolvedParticipantsLookup = resolvedParticipantDtos
                .GroupBy(dto => dto.DnsMessageId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(dto => new TrafficParticipantListModel
                    {
                        Id = dto.ParticipantId,
                        Address = dto.Address,
                        Port = dto.Port
                    }).ToList()
                );

            foreach (var packetModel in groupedPackets)
            {
                if (resolvedParticipantsLookup.TryGetValue(packetModel.Id, out var participants))
                {
                    packetModel.ResolvedTrafficParticipants = participants;
                }
            }
        }

        return groupedPackets;
    }
}
