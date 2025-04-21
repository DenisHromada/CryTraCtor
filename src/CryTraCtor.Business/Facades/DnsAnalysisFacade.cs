using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Services;

namespace CryTraCtor.Business.Facades;

public class DnsAnalysisFacade : IDnsAnalysisFacade
{
    private readonly IStoredFileFacade _storedFileFacade;
    private readonly DnsPacketReader _dnsPacketReader;
    private readonly IDnsPacketFacade _dnsPacketFacade;

    public DnsAnalysisFacade(
        IStoredFileFacade storedFileFacade,
        DnsPacketReader dnsPacketReader,
        IDnsPacketFacade dnsPacketFacade)
    {
        _storedFileFacade = storedFileFacade;
        _dnsPacketReader = dnsPacketReader;
        _dnsPacketFacade = dnsPacketFacade;
    }

    public async Task AnalyzeDnsPacketsAsync(Guid fileAnalysisId, Guid storedFileId)
    {
        var fileMetadata = await _storedFileFacade.GetByIdAsync(storedFileId);
        if (fileMetadata == null || string.IsNullOrEmpty(fileMetadata.InternalFilePath))
        {
            Console.Error.WriteLine($"[DnsAnalysisFacade] Stored file metadata not found or internal path missing for ID: {storedFileId}. Skipping DNS analysis for FileAnalysisId: {fileAnalysisId}");
            return;
        }
        var internalFilePath = fileMetadata.InternalFilePath;

        IEnumerable<IDnsPacketSummary> dnsPackets;
        try
        {
            dnsPackets = _dnsPacketReader.Read(internalFilePath);
        }
        catch (Exception ex)
        {
             Console.Error.WriteLine($"[DnsAnalysisFacade] Error reading DNS packets from {internalFilePath} for FileAnalysisId: {fileAnalysisId}. Error: {ex.Message}");
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
                    await _dnsPacketFacade.CreateOrUpdateAsync(dnsPacketModel);
                }
            }
            catch (Exception ex)
            {
                 Console.Error.WriteLine($"[DnsAnalysisFacade] Error processing DNS packet for FileAnalysisId: {fileAnalysisId}. TxId: {packetSummary?.TransactionId}. Error: {ex.Message}");
            }
        }
    }
}
