using CryTraCtor.Business.Models;
using CryTraCtor.Business.Models.Agregates;
using CryTraCtor.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryTraCtor.APi.Controllers.Dns;

[ApiController]
[Route("dns/queried-domains")]
public class QueriedDomainController(
    DomainDetector domainDetector,
    KnownDomainFilter knownDomainFilter,
    DnsTransactionSummaryModelTransformer transformer
) : Controller
{
    [HttpGet("all/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetAllQueriedDomains(string fileName)
    {
        var extractedDomainTransactions = await domainDetector.AnalyzeAsync(fileName);

        return transformer.TransformToDomainQueriers(extractedDomainTransactions);
    }
    
    [HttpGet("known/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetKnownQueriedDomains(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        return transformer.TransformToDomainQueriers(knownDomainTransactions);
    }

    [HttpGet("unknown/{fileName}")]
    public async Task<Dictionary<string, HashSet<string>>> GetUnknownQueriedDomains(string fileName)
    {
        var unknownDomainTransactions = await knownDomainFilter.GetUnknownAsync(fileName);
        
        return transformer.TransformToDomainQueriers(unknownDomainTransactions);
    }

    [HttpGet("known/groupedByProduct/{fileName}")]
    public async Task<IEnumerable<GroupedQueriedDomains>> GetGroupedKnownQueriedDomains(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        return await transformer.GroupByProduct(knownDomainTransactions);
    }
    
    [HttpGet("known/groupByClientAddress/{fileName}")]
    public async Task<ActionResult> GetDetectedDomainsByAddress(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        var detectedWallets = transformer.TransformToDetectedAddressWallets(knownDomainTransactions);

        return Ok(detectedWallets);
    }
    
    [HttpGet("known/groupByClientAddressPort/{fileName}")]
    public async Task<ActionResult> GetDetectedDomainsByAddressPort(string fileName)
    {
        var knownDomainTransactions = await knownDomainFilter.GetKnownAsync(fileName);
        var detectedWallets = transformer.TransformToDetectedAddressPortWallets(knownDomainTransactions);

        return Ok(detectedWallets);
    }
}