using System.Collections.ObjectModel;
using CryTraCtor.Analyzers;
using CryTraCtor.Facades;
using CryTraCtor.Mappers;
using CryTraCtor.Models.DnsTransaction;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/queried-domains")]
public class QueriedDomainController(
    IStoredFileFacade storedFileFacade
) : Controller
{
    [HttpGet("{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetAllQueriedDomains(string fileName)
    {
        var storedFileDetailModel = await storedFileFacade.GetFileMetadataAsync(fileName);
        var captureFilePath = storedFileDetailModel.InternalFilePath;

        var domainNameDetector = new DnsTransactionExtractor(captureFilePath);
        domainNameDetector.Run();

        return GetDomainQueriers(domainNameDetector.DnsTransactions);
    }
    
    [HttpGet("known/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetKnownQueriedDomains(string fileName)
    {
        var storedFileDetailModel = await storedFileFacade.GetFileMetadataAsync(fileName);
        var captureFilePath = storedFileDetailModel.InternalFilePath;

        var domainNameDetector = new DnsTransactionExtractor(captureFilePath);
        domainNameDetector.Run();
        var knownDomainDetector = new KnownDomainDetector(domainNameDetector.DnsTransactions);
        knownDomainDetector.Run();
        return GetDomainQueriers(knownDomainDetector.FilteredDnsTransactions);
    }

    private static Dictionary<string, HashSet<string>> GetDomainQueriers(
        Collection<DnsTransactionSummary> transactions)
    {
        Dictionary<string, HashSet<string>> queriedDomains = new();

        foreach (var dnsTransaction in transactions)
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
    
    [HttpGet("wallet-traffic/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetWalletTraffic(string fileName)
    {
        var storedFileDetailModel = await storedFileFacade.GetFileMetadataAsync(fileName);
        var captureFilePath = storedFileDetailModel.InternalFilePath;

        var domainNameDetector = new DnsTransactionExtractor(captureFilePath);
        domainNameDetector.Run();
        var knownDomainDetector = new KnownDomainDetector(domainNameDetector.DnsTransactions);
        knownDomainDetector.Run();

        var transactions = knownDomainDetector.FilteredDnsTransactions;
        
        Dictionary<string, HashSet<string>> queriedDomains = new();

        foreach (var dnsTransaction in transactions)
        {
            if (dnsTransaction.Query.RecordType == "A")
            {
                var domainDetail = KnownDomainDetector.GetKnownDomainDetail(dnsTransaction.Query.Name);
                if (domainDetail == null)
                {
                    continue;
                }

                var walletProvider = domainDetail.Product;
                if (queriedDomains.ContainsKey(walletProvider))
                {
                    if (queriedDomains[walletProvider].Contains(dnsTransaction.Client.ToString()))
                    {
                        continue;
                    }
                    else
                    {
                        queriedDomains[walletProvider].Add(dnsTransaction.Client.ToString());
                    }
                }
                else
                {
                    queriedDomains.Add(walletProvider, [dnsTransaction.Client.ToString()]);
                }
            }
        }

        return queriedDomains;
    }
}