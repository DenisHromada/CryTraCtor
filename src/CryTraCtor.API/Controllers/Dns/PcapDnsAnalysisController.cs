using CryTraCtor.Business.Models.Agregates;
using CryTraCtor.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/queried-domains")]
public class PcapDnsAnalysisController(
    DomainDetector domainDetector,
    KnownDomainFilter knownDomainFilter,
    DnsTransactionSummaryModelFormatter responseFormatter
) : Controller
{
    [HttpGet("all/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetAllQueriedDomains(string fileName)
    {
        var extractedDomainTransactions = await domainDetector.AnalyzeAsync(fileName);

        return responseFormatter.TransformToDomainQueriers(extractedDomainTransactions);
    }
    
    [HttpGet("known/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetKnownQueriedDomains(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        return responseFormatter.TransformToDomainQueriers(knownDomainTransactions);
    }

    [HttpGet("unknown/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetUnknownQueriedDomains(string fileName)
    {
        var unknownDomainTransactions = await knownDomainFilter.GetUnknownAsync(fileName);
        
        return responseFormatter.TransformToDomainQueriers(unknownDomainTransactions);
    }

    [HttpGet("known/groupByProduct/{fileName}")]
    public async Task<IEnumerable<GroupedQueriedDomains>> GetGroupedKnownQueriedDomains(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        return await responseFormatter.GroupByProduct(knownDomainTransactions);
    }
    
    [HttpGet("known/groupByClientAddress/{fileName}")]
    public async Task<ActionResult> GetDetectedDomainsByAddress(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        var detectedWallets = responseFormatter.TransformToDetectedAddressWallets(knownDomainTransactions);

        return Ok(detectedWallets);
    }
    
    [HttpGet("known/groupByClientAddressPort/{fileName}")]
    public async Task<ActionResult> GetDetectedDomainsByAddressPort(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        var detectedWallets = responseFormatter.TransformToDetectedAddressPortWallets(knownDomainTransactions);

        return Ok(detectedWallets);
    }
    
    [HttpGet("known/groupByClientAddressPortThenProduct/{fileName}")]
    public async Task<ActionResult> GetDetectedDomainsByAddressPortThenProduct(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        var detectedWallets = responseFormatter.TransformToDetectedAddressPortWallets(knownDomainTransactions);

        return Ok(detectedWallets);
    }
}