using System.Collections.ObjectModel;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Packet.Models;
using CryTraCtor.Packet.Services;

namespace CryTraCtor.Business.Services;

public class DomainDetector(
    IStoredFileFacade storedFileFacade,
    IDnsTransactionExtractor dnsTransactionExtractor
    ) 
{
    public async Task<Collection<DnsTransactionSummaryModel>> AnalyzeAsync(string fileName)
    {
        var internalFilePath = (await storedFileFacade.GetFileMetadataAsync(fileName)).InternalFilePath;
        var dnsTransactions = dnsTransactionExtractor.Run(internalFilePath);
        return dnsTransactions;
    }
    
}