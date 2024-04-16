using CryTraCtor.Analyzers;
using CryTraCtor.Facades;
using CryTraCtor.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/known-domain-count")]
public class KnownDomainCountController(
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
            knownDomainDetector.Run();

            return Ok(knownDomainDetector.KnownDomainDetails.Count);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}