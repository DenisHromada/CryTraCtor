using System.Collections.ObjectModel;
using CryTraCtor.APi.Models.Dns;
using CryTraCtor.Business.Models.CryptoProduct;
using CryTraCtor.Business.Services;
using CryTraCtor.Packet.Analyzers;
using CryTraCtor.Packet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/queried-domains")]
public class QueriedDomainController(
    DomainDetector domainDetector,
    KnownDomainDetector knownDomainDetector,
    DnsTransactionSummaryModelTransformer transformer
) : Controller
{
    [HttpGet("known-domain/{fileName}")]
    public async Task<ActionResult<string>> GetDomainCount(string fileName)
    {
        try
        {
            var extractedDomainNames = await domainDetector.AnalyzeAsync(fileName);

            var knownDomainDetector = new OLD_KnownDomainDetector(extractedDomainNames);
            knownDomainDetector.OldRun();

            var walletIpAddresses = knownDomainDetector.GetKnownDomainIpAddresses();

            var response = new Collection<KnownDomainResponseEntry>();
            foreach (var knownWalletKeyPair in walletIpAddresses)
            {
                var responseEntry = new KnownDomainResponseEntry(
                    knownWalletKeyPair.Key.DomainName,
                    knownWalletKeyPair.Key.Vendor,
                    knownWalletKeyPair.Key.Product,
                    knownWalletKeyPair.Key.Purpose,
                    knownWalletKeyPair.Key.Cryptocurrency,
                    knownWalletKeyPair.Key.Description,
                    knownWalletKeyPair.Value
                );
                response.Add(responseEntry);
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("all/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetAllQueriedDomains(string fileName)
    {
        var extractedDomainTransactions = await domainDetector.AnalyzeAsync(fileName);

        return transformer.TransformToDomainQueriers(extractedDomainTransactions);
    }
    
    [HttpGet("known/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetKnownQueriedDomains(string fileName)
    {
        var knownDomainTransactions = await knownDomainDetector.AnalyzeAsync(fileName);
        return transformer.TransformToDomainQueriers(knownDomainTransactions);
    }
    
    [HttpGet("grouped/{fileName}")]
    public async Task<Dictionary<CryptoProductListModel, Collection<DnsTransactionSummaryModel>>> GetGroupedKnownQueriedDomains(string fileName)
    {
        var knownDomainTransactions = await knownDomainDetector.AnalyzeAsync(fileName);
        return await transformer.GroupByProduct(knownDomainTransactions);
    }

    [HttpGet("wallet-traffic/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetWalletTraffic(string fileName)
    {
        var extractedDomainNames = await domainDetector.AnalyzeAsync(fileName);
        var knownDomainDetector = new OLD_KnownDomainDetector(extractedDomainNames);
        knownDomainDetector.Run();

        var transactions = knownDomainDetector.FilteredDnsTransactions;

        Dictionary<string, HashSet<string>> queriedDomains = new();

        foreach (var dnsTransaction in transactions)
        {
            if (dnsTransaction.Query.RecordType == "A")
            {
                var domainDetail = OLD_KnownDomainDetector.GetKnownDomainDetail(dnsTransaction.Query.Name);
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