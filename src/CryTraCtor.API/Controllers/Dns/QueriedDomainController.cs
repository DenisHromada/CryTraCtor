using System.Collections.ObjectModel;
using CryTraCtor.APi.Models.Dns;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Packet.Analyzers;
using CryTraCtor.Packet.DataTypeMappers;
using CryTraCtor.Packet.DataTypes.DnsTransaction;
using CryTraCtor.Packet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/queried-domains")]
public class QueriedDomainController(
    IStoredFileFacade storedFileFacade
) : Controller
{
    [HttpGet("known-domain/{fileName}")]
    public async Task<ActionResult<string>> GetDomainCount(string fileName)
    {
        try
        {
            var storedFileDetailModel = await storedFileFacade.GetFileMetadataAsync(fileName);
            var captureFilePath = storedFileDetailModel.InternalFilePath;

            var domainNameDetector = new DnsTransactionExtractor(captureFilePath);
            domainNameDetector.Run();

            var knownDomainDetector = new KnownDomainDetector(domainNameDetector.DnsTransactions);
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
        Collection<DnsTransactionSummaryModel> transactions)
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