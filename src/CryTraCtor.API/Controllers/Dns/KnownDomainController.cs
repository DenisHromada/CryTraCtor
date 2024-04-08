using System.Collections.ObjectModel;
using CryTraCtor.Analyzers;
using CryTraCtor.APi.Models.Dns;
using CryTraCtor.Helpers;
using CryTraCtor.Mappers;
using CryTraCtor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/known-domain")]
public class KnownDomainController : Controller
{
    [HttpGet("{fileId}")]
    public async Task<ActionResult<string>> GetDomainCount(string fileId)
    {
        try
        {
            var captureFilePath = GetFileFromId.GetFilepathFromId(fileId);

            var domainNameDetector = new DnsTransactionExtractor(captureFilePath);
            domainNameDetector.Run();

            var knownDomainDetector = new KnownDomainDetector(domainNameDetector.DnsTransactions);
            knownDomainDetector.Run();

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
            return BadRequest();
        }
    }
}