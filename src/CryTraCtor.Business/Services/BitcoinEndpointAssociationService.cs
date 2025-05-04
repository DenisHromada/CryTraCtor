using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.Bitcoin;
using CryTraCtor.Business.Models.Bitcoin;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Services;

public class BitcoinEndpointAssociationService(
    ITrafficParticipantFacade trafficParticipantFacade,
    ILogger<BitcoinEndpointAssociationService> logger,
    BitcoinTransactionMapper bitcoinTransactionMapper,
    BitcoinBlockHeaderMapper bitcoinBlockHeaderMapper)
{
    public async Task<BitcoinPacketDetailModel?> AssociateAsync(Guid fileAnalysisId,
        BitcoinMessageSummary concreteSummary)
    {
        var senderParticipant = await trafficParticipantFacade.GetByAddressPortAndFileAnalysisIdAsync(
            fileAnalysisId,
            concreteSummary.Source.Address,
            concreteSummary.Source.Port
        );

        var recipientParticipant = await trafficParticipantFacade.GetByAddressPortAndFileAnalysisIdAsync(
            fileAnalysisId,
            concreteSummary.Destination.Address,
            concreteSummary.Destination.Port);

        var senderId = senderParticipant?.Id ?? Guid.Empty;
        var recipientId = recipientParticipant?.Id ?? Guid.Empty;

        if (senderId == Guid.Empty || recipientId == Guid.Empty)
        {
            logger.LogWarning(
                "[BitcoinEndpointAssociationService] Could not find sender ({SourceAddress}:{SourcePort}) or recipient ({DestinationAddress}:{DestinationPort}) participant ID in lookup for FileAnalysisId: {FileAnalysisId}. Skipping Bitcoin message: {Command}",
                concreteSummary.Source.Address, concreteSummary.Source.Port, concreteSummary.Destination.Address,
                concreteSummary.Destination.Port, fileAnalysisId, concreteSummary.Command);
            return null;
        }

        var bitcoinPacketModel = new BitcoinPacketDetailModel
        {
            FileAnalysisId = fileAnalysisId,
            SenderId = senderId,
            RecipientId = recipientId,
            Timestamp = concreteSummary.Timestamp,
            Magic = concreteSummary.Magic,
            Command = concreteSummary.Command,
            Length = concreteSummary.PayloadSize,
            Checksum = concreteSummary.Checksum,
            Inventories = concreteSummary.Inventories?
                .Select(inv => new BitcoinInventoryItemListModel
                {
                    Type = inv.Type.ToString(),
                    Hash = inv.Hash.ToString()
                }).ToList()
        };

        if (concreteSummary.Transaction != null)
        {
            bitcoinPacketModel.Transaction = bitcoinTransactionMapper.Map(concreteSummary.Transaction);
        }

        if (concreteSummary.Headers != null)
        {
            bitcoinPacketModel.Headers = concreteSummary.Headers
                .Select(h => bitcoinBlockHeaderMapper.Map(h))
                .ToList();
        }

        return bitcoinPacketModel;
    }
}
