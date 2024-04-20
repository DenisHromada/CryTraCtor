using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Packet.Services;

namespace CryTraCtor.Business.Services;

public class KnownDomainDetector(
    IKnownDomainFacade knownDomainFacade,
    IStoredFileFacade storedFileFacade,
    IDnsTransactionExtractor dnsTransactionExtractor
    )
{
    public async void Analyze(string fileName)
    {
        var internalFilePath = (await storedFileFacade.GetFileMetadataAsync(fileName)).InternalFilePath;
        var dnsTransactions = dnsTransactionExtractor.Run(internalFilePath);
        var knownDomains = await knownDomainFacade.GetAllAsync();
    }
    
}