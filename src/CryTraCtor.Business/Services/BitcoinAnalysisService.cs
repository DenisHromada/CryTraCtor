using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Services;

public class BitcoinAnalysisService(
    BitcoinPacketReader bitcoinPacketReader,
    IBitcoinPacketFacade bitcoinPacketFacade,
    ITrafficParticipantFacade trafficParticipantFacade,
    ILogger<BitcoinAnalysisService> logger)
{
    public async Task AnalyzeAsync(StoredFileDetailModel storedFile, Guid fileAnalysisId)
    {
        if (string.IsNullOrEmpty(storedFile.InternalFilePath))
        {
            logger.LogError(
                "[BitcoinAnalysisService] Stored file internal path missing for ID: {StoredFileId}. Skipping Bitcoin analysis for FileAnalysisId: {FileAnalysisId}",
                storedFile.Id, fileAnalysisId);
            return;
        }

        var internalFilePath = storedFile.InternalFilePath;
        logger.LogInformation(
            "[BitcoinAnalysisService] Starting Bitcoin analysis for FileAnalysisId: {FileAnalysisId} on file: {FilePath}",
            fileAnalysisId, internalFilePath);

        IEnumerable<IBitcoinPacketSummary> bitcoinPackets;
        try
        {
            bitcoinPackets = bitcoinPacketReader.Read(internalFilePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[BitcoinAnalysisService] Error reading Bitcoin packets from {InternalFilePath} for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                internalFilePath, fileAnalysisId, ex.Message);
            return;
        }

        var participants = await trafficParticipantFacade.GetByFileAnalysisIdAsync(fileAnalysisId);
        var participantLookup = participants.ToDictionary(
            p => $"{p.Address}:{p.Port}",
            p => p.Id
        );

        int processedCount = 0;
        foreach (var packetSummary in bitcoinPackets)
        {
            try
            {
                var sourceKey = $"{packetSummary.Source.Address}:{packetSummary.Source.Port}";
                var destinationKey = $"{packetSummary.Destination.Address}:{packetSummary.Destination.Port}";

                participantLookup.TryGetValue(sourceKey, out var senderId);
                participantLookup.TryGetValue(destinationKey, out var recipientId);

                if (senderId == Guid.Empty || recipientId == Guid.Empty)
                {
                    logger.LogWarning(
                        "[BitcoinAnalysisService] Could not find sender ({SourceKey}) or recipient ({DestinationKey}) participant ID in lookup for FileAnalysisId: {FileAnalysisId}. Skipping Bitcoin packet at {Timestamp}",
                        sourceKey, destinationKey, fileAnalysisId, packetSummary.Timestamp);
                    continue;
                }

                var bitcoinPacketModel = new BitcoinPacketDetailModel
                {
                    FileAnalysisId = fileAnalysisId,
                    SenderId = senderId,
                    RecipientId = recipientId,
                    Timestamp = packetSummary.Timestamp
                };

                await bitcoinPacketFacade.CreateOrUpdateAsync(bitcoinPacketModel);
                processedCount++;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "[BitcoinAnalysisService] Error processing Bitcoin packet for FileAnalysisId: {FileAnalysisId} at {Timestamp}. Error: {ErrorMessage}",
                    fileAnalysisId, packetSummary?.Timestamp, ex.Message);
            }
        }

        logger.LogInformation(
            "[BitcoinAnalysisService] Completed Bitcoin analysis for FileAnalysisId: {FileAnalysisId}. Processed {ProcessedCount} packets.",
            fileAnalysisId, processedCount);
    }
}
