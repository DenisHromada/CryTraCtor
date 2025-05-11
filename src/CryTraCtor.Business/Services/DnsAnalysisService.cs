using System.Diagnostics;
using System.Text;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models;
using CryTraCtor.Business.Models.FileAnalysis;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Models;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Services;

public class DnsAnalysisService(
    DnsPacketReader dnsPacketReader,
    IDnsMessageFacade dnsMessageFacade,
    ITrafficParticipantFacade trafficParticipantFacade,
    DomainMatchAssociationService domainMatchAssociationService,
    ILogger<DnsAnalysisService> logger,
    GenericPacketReaderService genericPacketReaderService,
    IGenericPacketFacade genericPacketFacade)
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


        var participantsEnumerable = await trafficParticipantFacade.GetByFileAnalysisIdAsync(fileAnalysisId);
        var participantsList = participantsEnumerable.ToList();
        var participantLookup = participantsList.ToDictionary(
            p => $"{p.Address}:{p.Port}",
            p => p
        );

        var targetEndpointsForFilter = new HashSet<InternetEndpointModel>();

        foreach (var packetSummary in dnsPackets)
        {
            DnsMessageModel? dnsPacketModel = null;

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
                        dnsPacketModel = new DnsMessageModel
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
                        };
                    }
                }
                else if (packetSummary is DnsPacketResponse { Query.RecordType: "A" or "AAAA" } response)
                {
                    var responseIpAddresses = response.Answers
                        .Where(a => a.RecordType is "A" or "AAAA"
                                    && !string.IsNullOrEmpty(a.RecordData))
                        .Select(a => a.RecordData)
                        .ToList();

                    if (responseIpAddresses.Count != 0)
                    {
                        dnsPacketModel = new DnsMessageModel
                        {
                            Id = Guid.NewGuid(),
                            FileAnalysisId = fileAnalysisId,
                            SenderId = sender.Id,
                            RecipientId = recipient.Id,
                            Timestamp = packetSummary.Timestamp,
                            TransactionId = (ushort)response.TransactionId,
                            IsQuery = false,
                            QueryName = response.Query.Name ?? string.Empty,
                            QueryType = response.Query.RecordType
                        };

                        foreach (var ipAddressString in responseIpAddresses)
                        {
                            var existingParticipants =
                                participantsList.Where(p => p.Address == ipAddressString).ToList();

                            if (existingParticipants.Count != 0)
                            {
                                foreach (var participant in existingParticipants)
                                {
                                    dnsPacketModel.ResolvedTrafficParticipants.Add(participant);

                                    if (participant.Port != 0)
                                    {
                                        targetEndpointsForFilter.Add(
                                            new InternetEndpointModel(participant.Address, participant.Port));
                                    }
                                }
                            }
                            else
                            {
                                var newDetailParticipant = new TrafficParticipantDetailModel
                                {
                                    Address = ipAddressString,
                                    Port = 0,
                                    FileAnalysis = new FileAnalysisListModel { Id = fileAnalysisId }
                                };


                                var createdOrUpdatedDetailParticipant =
                                    await trafficParticipantFacade.CreateOrUpdateAsync(newDetailParticipant);


                                var newListParticipant = new TrafficParticipantListModel
                                {
                                    Id = createdOrUpdatedDetailParticipant.Id,
                                    Address = createdOrUpdatedDetailParticipant.Address,
                                    Port = createdOrUpdatedDetailParticipant.Port,
                                    FileAnalysisId = fileAnalysisId
                                };

                                dnsPacketModel.ResolvedTrafficParticipants.Add(newListParticipant);

                                if (newListParticipant.Port != 0)
                                {
                                    targetEndpointsForFilter.Add(new InternetEndpointModel(newListParticipant.Address,
                                        newListParticipant.Port));
                                }


                                participantsList.Add(newListParticipant);
                                var newParticipantKey = $"{newListParticipant.Address}:{newListParticipant.Port}";
                                participantLookup.TryAdd(newParticipantKey, newListParticipant);
                            }
                        }
                    }
                }

                if (dnsPacketModel != null)
                {
                    await dnsMessageFacade.CreateOrUpdateAsync(dnsPacketModel);
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
            var stopwatch = Stopwatch.StartNew();
            await domainMatchAssociationService.AnalyseAsync(fileAnalysisId);
            stopwatch.Stop();
            logger.LogInformation(
                "[DnsAnalysisService] Completed domain association for FileAnalysisId: {FileAnalysisId} in {ElapsedMilliseconds} ms",
                fileAnalysisId, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[DnsAnalysisService] Error during domain association for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                fileAnalysisId, ex.Message);
        }


        await CreateGenericPacketsForDnsResolvedEndpointsAsync(internalFilePath, fileAnalysisId,
            targetEndpointsForFilter, participantsList, participantLookup);
    }

    private async Task CreateGenericPacketsForDnsResolvedEndpointsAsync(
        string pcapFilePath,
        Guid fileAnalysisId,
        HashSet<InternetEndpointModel> targetEndpoints,
        List<TrafficParticipantListModel> currentParticipantsList,
        Dictionary<string, TrafficParticipantListModel> currentParticipantLookup)
    {
        logger.LogInformation(
            "[DnsAnalysisService] Starting generic packet creation for DNS resolved endpoints for FileAnalysisId: {FileAnalysisId}",
            fileAnalysisId);
        try
        {
            if (targetEndpoints.Count == 0)
            {
                logger.LogInformation(
                    "[DnsAnalysisService] No DNS resolved target endpoints with non-zero port found for FileAnalysisId: {FileAnalysisId}. Skipping generic packet creation.",
                    fileAnalysisId);
                return;
            }


            var bpfFilterBuilder = new StringBuilder();
            foreach (var endpoint in targetEndpoints)
            {
                if (bpfFilterBuilder.Length > 0)
                {
                    bpfFilterBuilder.Append(" or ");
                }

                bpfFilterBuilder.Append($"(host {endpoint.Address} and port {endpoint.Port})");
            }

            var bpfFilter = bpfFilterBuilder.ToString();
            logger.LogDebug("[DnsAnalysisService] Constructed BPF filter for generic packets: {BpfFilter}", bpfFilter);


            var genericPacketSummaries = genericPacketReaderService.ReadIpPackets(pcapFilePath, bpfFilter);


            int genericPacketsCreated = 0;
            foreach (var packetSummary in genericPacketSummaries)
            {
                try
                {
                    var sourceKey = $"{packetSummary.Source.Address}:{packetSummary.Source.Port}";
                    if (!currentParticipantLookup.TryGetValue(sourceKey, out var sender))
                    {
                        var newSenderDetail = new TrafficParticipantDetailModel
                        {
                            Address = packetSummary.Source.Address, Port = packetSummary.Source.Port,
                            FileAnalysis = new FileAnalysisListModel { Id = fileAnalysisId }
                        };
                        var persistedSender = await trafficParticipantFacade.CreateOrUpdateAsync(newSenderDetail);
                        sender = new TrafficParticipantListModel
                        {
                            Id = persistedSender.Id, Address = persistedSender.Address, Port = persistedSender.Port,
                            FileAnalysisId = fileAnalysisId
                        };
                        currentParticipantsList.Add(sender);
                        currentParticipantLookup[sourceKey] = sender;
                    }


                    var destinationKey = $"{packetSummary.Destination.Address}:{packetSummary.Destination.Port}";
                    if (!currentParticipantLookup.TryGetValue(destinationKey, out var recipient))
                    {
                        var newRecipientDetail = new TrafficParticipantDetailModel
                        {
                            Address = packetSummary.Destination.Address, Port = packetSummary.Destination.Port,
                            FileAnalysis = new FileAnalysisListModel { Id = fileAnalysisId }
                        };
                        var persistedRecipient = await trafficParticipantFacade.CreateOrUpdateAsync(newRecipientDetail);
                        recipient = new TrafficParticipantListModel
                        {
                            Id = persistedRecipient.Id, Address = persistedRecipient.Address,
                            Port = persistedRecipient.Port, FileAnalysisId = fileAnalysisId
                        };
                        currentParticipantsList.Add(recipient);
                        currentParticipantLookup[destinationKey] = recipient;
                    }


                    var genericPacketModel = new GenericPacketModel
                    {
                        Id = Guid.NewGuid(),
                        FileAnalysisId = fileAnalysisId,
                        SenderId = sender.Id,
                        RecipientId = recipient.Id,
                        Timestamp = packetSummary.Timestamp
                    };


                    await genericPacketFacade.CreateOrUpdateAsync(genericPacketModel);
                    genericPacketsCreated++;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,
                        "[DnsAnalysisService] Error processing a generic packet summary for FileAnalysisId: {FileAnalysisId}. Source: {Source}, Dest: {Destination}, Timestamp: {Timestamp}. Error: {ErrorMessage}",
                        fileAnalysisId, packetSummary.Source, packetSummary.Destination, packetSummary.Timestamp,
                        ex.Message);
                }
            }

            logger.LogInformation(
                "[DnsAnalysisService] Completed generic packet creation for FileAnalysisId: {FileAnalysisId}. {Count} packets created.",
                fileAnalysisId, genericPacketsCreated);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[DnsAnalysisService] Error during generic packet creation for DNS resolved endpoints for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}",
                fileAnalysisId, ex.Message);
        }
    }
}
