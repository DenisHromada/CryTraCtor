using CryTraCtor.Facades;
using CryTraCtor.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/transaction-count")]
public class TransactionCountController(
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
            return Ok(domainNameDetector.DnsTransactions.Count);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}