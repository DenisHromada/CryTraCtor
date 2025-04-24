using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Services;

public class DnsAnalysisService(
    DnsPacketReader dnsPacketReader,
    IDnsPacketFacade dnsPacketFacade,
    ITrafficParticipantFacade trafficParticipantFacade,
    DomainMatchAssociationService domainMatchAssociationService,
    ILogger<DnsAnalysisService> logger)
{
    public async Task AnalyzeAsync(StoredFileDetailModel storedFile, Guid fileAnalysisId)
    {
        if (string.IsNullOrEmpty(storedFile.InternalFilePath))
        {
            logger.LogError(
                "[DnsAnalysisService] Stored file internal path missing for ID: {StoredFileId}. Skipping DNS analysis for FileAnalysisId: {FileAnalysisId}",
                storedFile.Id, fileAnalysisId);
            return;
        }

        var internalFilePath = storedFile.InternalFilePath;

        IEnumerable<IDnsPacketSummary> dnsPackets;
        try
        {
            dnsPackets = dnsPacketReader.Read(internalFilePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[DnsAnalysisService] Error reading DNS packets from {InternalFilePath} for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                internalFilePath, fileAnalysisId, ex.Message);
            return;
        }


        var participants = await trafficParticipantFacade.GetByFileAnalysisIdAsync(fileAnalysisId);
        var participantLookup = participants.ToDictionary(
            p => $"{p.Address}:{p.Port}",
            p => p
        );

        foreach (var packetSummary in dnsPackets)
        {
            DnsPacketModel? dnsPacketModel = null;

            try
            {
                var sourceKey = $"{packetSummary.Source.Address}:{packetSummary.Source.Port}";
                var destinationKey = $"{packetSummary.Destination.Address}:{packetSummary.Destination.Port}";

                participantLookup.TryGetValue(sourceKey, out var sender);
                participantLookup.TryGetValue(destinationKey, out var recipient);

                if (sender == null || recipient == null)
                {
                    logger.LogWarning(
                        "[DnsAnalysisService] Could not find sender ({SourceKey}) or recipient ({DestinationKey}) participant in lookup for FileAnalysisId: {FileAnalysisId}. Skipping DNS packet TxId: {TransactionId}",
                        sourceKey, destinationKey, fileAnalysisId, packetSummary?.TransactionId);
                    continue;
                }

                if (packetSummary is DnsPacketQuery query)
                {
                    if (query.Query is { RecordType: "A" or "AAAA" })
                    {
                        dnsPacketModel = new DnsPacketModel
                        {
                            Id = Guid.NewGuid(),
                            FileAnalysisId = fileAnalysisId,
                            SenderId = sender.Id,
                            RecipientId = recipient.Id,
                            Timestamp = packetSummary.Timestamp,
                            TransactionId = (ushort)query.TransactionId,
                            IsQuery = true,
                            QueryName = query.Query.Name ?? string.Empty,
                            QueryType = query.Query.RecordType,
                            ResponseAddresses = null
                        };
                    }
                }
                else if (packetSummary is DnsPacketResponse response)
                {
                    if (response.Query != null &&
                        response.Query.RecordType is "A" or "AAAA")
                    {
                        var addresses = response.Answers
                            .Where(a => a.RecordType is "A" or "AAAA" &&
                                        !string.IsNullOrEmpty(a.RecordData))
                            .Select(a => a.RecordData)
                            .ToList();

                        if (addresses.Any())
                        {
                            dnsPacketModel = new DnsPacketModel
                            {
                                Id = Guid.NewGuid(),
                                FileAnalysisId = fileAnalysisId,
                                SenderId = sender.Id,
                                RecipientId = recipient.Id,
                                Timestamp = packetSummary.Timestamp,
                                TransactionId = (ushort)response.TransactionId,
                                IsQuery = false,
                                QueryName = response.Query.Name ?? string.Empty,
                                QueryType = response.Query.RecordType,
                                ResponseAddresses = string.Join(",", addresses)
                            };
                        }
                    }
                }

                if (dnsPacketModel != null)
                {
                    await dnsPacketFacade.CreateOrUpdateAsync(dnsPacketModel);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "[DnsAnalysisService] Error processing DNS packet for FileAnalysisId: {FileAnalysisId}. TxId: {TransactionId}. Error: {ErrorMessage}",
                    fileAnalysisId, packetSummary?.TransactionId, ex.Message);
            }
        }

        try
        {
            logger.LogInformation(
                "[DnsAnalysisService] Starting domain association for FileAnalysisId: {FileAnalysisId}",
                fileAnalysisId);
            await domainMatchAssociationService.AnalyseAsync(fileAnalysisId);
            logger.LogInformation(
                "[DnsAnalysisService] Completed domain association for FileAnalysisId: {FileAnalysisId}",
                fileAnalysisId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[DnsAnalysisService] Error during domain association for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                fileAnalysisId, ex.Message);
        }
    }
}
