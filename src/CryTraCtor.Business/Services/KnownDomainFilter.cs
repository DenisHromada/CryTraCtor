using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Business.Services;

public class KnownDomainFilter(
    DomainDetector detector,
    IKnownDomainFacade knownDomainFacade
)
{
    public async Task<ICollection<DnsTransactionSummaryModel>> GetKnownAsync(string fileName)
    {
        var allQueried= await detector.AnalyzeAsync(fileName);
        var allKnown= await knownDomainFacade.GetAllAsync();

        var joinQuery = 
                from query in allQueried
                join known in allKnown
                    on query.Query.Name equals known.DomainName
                    select query
                ;
        return joinQuery.ToList();
    }
    
    public async Task<ICollection<DnsTransactionSummaryModel>> GetUnknownAsync(string fileName)
    {
        var allQueried= await detector.AnalyzeAsync(fileName);
        var allKnown= await knownDomainFacade.GetAllAsync();

        var joinQuery = 
                from query in allQueried
                where allKnown.All(known => known.DomainName != query.Query.Name)
                select query
                ;
        return joinQuery.ToList();
    }
}