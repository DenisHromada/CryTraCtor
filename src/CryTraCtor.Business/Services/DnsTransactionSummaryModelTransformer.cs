using System.Collections.ObjectModel;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models;
using CryTraCtor.Business.Models.CryptoProduct;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Business.Services;

public class DnsTransactionSummaryModelTransformer(
    IKnownDomainFacade knownDomainFacade
)
{
    public Dictionary<string, HashSet<string>> TransformToDomainQueriers(
        Collection<DnsTransactionSummaryModel> dnsTransactionSummaryModels)
    {
        Dictionary<string, HashSet<string>> queriedDomains = new();

        foreach (var dnsTransaction in dnsTransactionSummaryModels)
        {
            if (dnsTransaction.Query.RecordType == "A")
            {
                var domainName = dnsTransaction.Query.Name;
                if (queriedDomains.ContainsKey(domainName))
                {
                    if (queriedDomains[domainName].Contains(dnsTransaction.Client.ToString()))
                    {
                        continue;
                    }
                    else
                    {
                        queriedDomains[domainName].Add(dnsTransaction.Client.ToString());
                    }
                }
                else
                {
                    queriedDomains.Add(domainName, [dnsTransaction.Client.ToString()]);
                }
            }
        }

        return queriedDomains;
    }

    public async Task<IEnumerable<GroupedQueriedDomains>> GroupByProduct(
        Collection<DnsTransactionSummaryModel> dnsTransactionSummaryModels)
    {
        Dictionary<CryptoProductListModel, Collection<DnsTransactionSummaryModel>> groupedTransactions = new();
        var allKnownDomains = await knownDomainFacade.GetAllDetailAsync();

        var joinQuery =
                from query in dnsTransactionSummaryModels
                join known in allKnownDomains
                    on query.Query.Name equals known.DomainName
                group query by known.CryptoProduct
            ;
        var transformedJoinQuery = joinQuery.Select(
            group => new GroupedQueriedDomains(group.Key, group.ToList()));

        return transformedJoinQuery;
    }
}