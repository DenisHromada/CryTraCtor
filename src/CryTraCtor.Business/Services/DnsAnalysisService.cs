using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Business.Services;

public class DnsAnalysisService(
    IStoredFileFacade storedFileFacade,
    DnsPacketReader dnsPacketReader,
    IDnsPacketFacade dnsPacketFacade,
    ILogger<DnsAnalysisService> logger)
{
    public async Task AnalyzeDnsPacketsAsync(Guid fileAnalysisId, Guid storedFileId)
    {
        var fileMetadata = await storedFileFacade.GetByIdAsync(storedFileId);
        if (fileMetadata == null || string.IsNullOrEmpty(fileMetadata.InternalFilePath))
        {
            logger.LogError("[DnsAnalysisService] Stored file metadata not found or internal path missing for ID: {StoredFileId}. Skipping DNS analysis for FileAnalysisId: {FileAnalysisId}", storedFileId, fileAnalysisId);
            return;
        }
        var internalFilePath = fileMetadata.InternalFilePath;

        IEnumerable<IDnsPacketSummary> dnsPackets;
        try
        {
            dnsPackets = dnsPacketReader.Read(internalFilePath);
        }
        catch (Exception ex)
        {
             logger.LogError(ex, "[DnsAnalysisService] Error reading DNS packets from {InternalFilePath} for FileAnalysisId: {FileAnalysisId}. Error: {ErrorMessage}", internalFilePath, fileAnalysisId, ex.Message);
             return;
        }

        foreach (var packetSummary in dnsPackets)
        {
            DnsPacketModel? dnsPacketModel = null;

            try
            {
                if (packetSummary is DnsPacketQuery query)
                {
                    if (query.Query != null && (query.Query.RecordType == "A" || query.Query.RecordType == "AAAA"))
                    {
                        dnsPacketModel = new DnsPacketModel
                        {
                            Id = Guid.NewGuid(),
                            FileAnalysisId = fileAnalysisId,
                            Timestamp = DateTime.UtcNow,
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
                    if (response.Query != null && (response.Query.RecordType == "A" || response.Query.RecordType == "AAAA"))
                    {
                        var addresses = response.Answers
                            .Where(a => (a.RecordType == "A" || a.RecordType == "AAAA") && !string.IsNullOrEmpty(a.RecordData))
                            .Select(a => a.RecordData)
                            .ToList();

                        if (addresses.Any())
                        {
                            dnsPacketModel = new DnsPacketModel
                            {
                                Id = Guid.NewGuid(),
                                FileAnalysisId = fileAnalysisId,
                                Timestamp = DateTime.UtcNow,
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
                 logger.LogError(ex, "[DnsAnalysisService] Error processing DNS packet for FileAnalysisId: {FileAnalysisId}. TxId: {TransactionId}. Error: {ErrorMessage}", fileAnalysisId, packetSummary?.TransactionId, ex.Message);
            }
        }
    }
}
