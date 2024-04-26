using CryTraCtor.Business.Models;
using CryTraCtor.Business.Services;
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
    public async Task<IEnumerable<GroupedQueriedDomains>> GetGroupedKnownQueriedDomains(string fileName)
    {
        var knownDomainTransactions = await knownDomainDetector.AnalyzeAsync(fileName);
        return await transformer.GroupByProduct(knownDomainTransactions);
    }
}