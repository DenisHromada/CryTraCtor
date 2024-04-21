using System.Collections.ObjectModel;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Business.Services;

public class KnownDomainDetector(
    DomainDetector detector,
    IKnownDomainFacade knownDomainFacade
)
{
    public async Task<Collection<DnsTransactionSummaryModel>> AnalyzeAsync(string fileName)
    {
        var allQueried= await detector.AnalyzeAsync(fileName);
        var allKnown= await knownDomainFacade.GetAllAsync();

        var joinQuery = 
                from query in allQueried
                join known in allKnown
                    on query.Query.Name equals known.DomainName
                    select new { query }
                ;
        var result = new Collection<DnsTransactionSummaryModel>(joinQuery.Select(j => j.query).ToList());
        return result;
    }
}