using System.Collections.ObjectModel;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Packet.Models;
using CryTraCtor.Packet.Services;

namespace CryTraCtor.Business.Services;

public class DomainDetector(
    IStoredFileFacade storedFileFacade,
    DnsTransactionExtractor dnsTransactionExtractor
)
{
    public async Task<Collection<DnsTransactionSummaryModel>> AnalyzeAsync(string fileName)
    {
        var fileMetadata = await storedFileFacade.GetFileMetadataAsync(fileName);
        if (fileMetadata == null)
        {
            Console.WriteLine(
                $"Warning: Could not find file metadata for '{fileName}' in DomainDetector.AnalyzeAsync.");
            return [];
        }

        var internalFilePath = fileMetadata.InternalFilePath;
        var dnsTransactions = dnsTransactionExtractor.Run(internalFilePath);
        return dnsTransactions;
    }
}
