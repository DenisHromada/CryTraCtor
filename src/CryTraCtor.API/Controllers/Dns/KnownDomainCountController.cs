using CryTraCtor.Analyzers;
using CryTraCtor.Helpers;
using CryTraCtor.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/known-domain-count")]
public class KnownDomainCountController : Controller
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

            return Ok(knownDomainDetector.KnownDomainDetails.Count);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}