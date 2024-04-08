using CryTraCtor.Helpers;
using CryTraCtor.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/transaction-count")]
public class TransactionCountController : Controller
{
    [HttpGet("{fileId}")]
    public async Task<ActionResult<string>> GetDomainCount(string fileId)
    {
        try
        {
            var captureFilePath = GetFileFromId.GetFilepathFromId(fileId);
            
            var domainNameDetector = new DnsTransactionExtractor(captureFilePath);
            domainNameDetector.Run();
            return Ok(domainNameDetector.DnsTransactions.Count);
            
        } catch (Exception e)
        {
            return BadRequest();
        }
        
    }
}