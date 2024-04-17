using System.Collections.ObjectModel;
using CryTraCtor.Analyzers;
using CryTraCtor.APi.Models.Dns;
using CryTraCtor.Facades;
using CryTraCtor.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/known-domain")]
public class KnownDomainController(
    IStoredFileFacade storedFileFacade
    ) : Controller
{
    [HttpGet("{fileName}")]
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
            return BadRequest();
        }
    }
}