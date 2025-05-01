using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Services;

public class BitcoinAnalysisService(
    IBitcoinPacketFacade bitcoinPacketFacade,
    ILogger<BitcoinAnalysisService> logger,
    BitcoinPacketAnalyzer bitcoinPacketAnalyzer,
    BitcoinEndpointAssociationService bitcoinEndpointAssociationService)
{
    public async Task AnalyzeAsync(StoredFileDetailModel storedFile, Guid fileAnalysisId)
    {
        logger.LogInformation(
            "[BitcoinAnalysisService] Starting Bitcoin analysis for FileAnalysisId: {FileAnalysisId} on file: {FilePath}",
            fileAnalysisId, storedFile.InternalFilePath);

        IEnumerable<IBitcoinPacketSummary> bitcoinMessages;
        try
        {
            bitcoinMessages = bitcoinPacketAnalyzer.AnalyzeFromFile(storedFile.InternalFilePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[BitcoinAnalysisService] Error processing Bitcoin data chunks from {InternalFilePath} for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                storedFile.InternalFilePath, fileAnalysisId, ex.Message);
            return;
        }

        var processedMessageCount = 0;
        foreach (var messageSummary in bitcoinMessages)
        {
            try
            {
                if (messageSummary is not BitcoinMessageSummary concreteSummary)
                {
                    logger.LogWarning(
                        "[BitcoinAnalysisService] Received unexpected summary type for FileAnalysisId: {FileAnalysisId}. Skipping.",
                        fileAnalysisId);
                    continue;
                }

                var bitcoinPacketModel = await bitcoinEndpointAssociationService.AssociateAsync(fileAnalysisId, concreteSummary);

                if (bitcoinPacketModel == null)
                {
                    continue;
                }

                await bitcoinPacketFacade.CreateOrUpdateAsync(bitcoinPacketModel);
                processedMessageCount++;
            }
            catch (Exception ex)
            {
                logger.LogError(0, ex,
                    "[BitcoinAnalysisService] Error processing Bitcoin message summary (Command: {Command}) for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                    messageSummary?.GetSerializedPacketString() ?? "unknown", fileAnalysisId, ex.Message);
            }
        }


        logger.LogInformation(
            "[BitcoinAnalysisService] Completed Bitcoin analysis for FileAnalysisId: {FileAnalysisId}. Processed {ProcessedMessageCount} messages.",
            fileAnalysisId, processedMessageCount);
    }
}
