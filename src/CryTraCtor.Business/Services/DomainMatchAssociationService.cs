using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Models.DomainMatch;
using CryTraCtor.Database.Enums;

namespace CryTraCtor.Business.Services
{
    public class DomainMatchAssociationService(
        IKnownDomainFacade knownDomainFacade,
        IDnsPacketFacade dnsPacketFacade,
        IDomainMatchFacade domainMatchFacade)
    {
        public async Task AnalyseAsync(Guid fileAnalysisId)
        {
            var knownDomains = await knownDomainFacade.GetAllAsync();
            if (!knownDomains.Any())
            {
                return;
            }

            var allDnsPackets = await dnsPacketFacade.GetAllAsync();
            var dnsPacketsForAnalysis = allDnsPackets.Where(p => p.FileAnalysisId == fileAnalysisId).ToList();

            if (!dnsPacketsForAnalysis.Any())
            {
                return;
            }

            var matchesToCreate = new List<DomainMatchModel>();

            foreach (var packet in dnsPacketsForAnalysis)
            {
                if (string.IsNullOrWhiteSpace(packet.QueryName)) continue;

                var queryNameLower = packet.QueryName.ToLowerInvariant().TrimEnd('.');
                bool foundExactMatch = false;
                foreach (var knownDomain in knownDomains)
                {
                    if (knownDomain.DomainName.ToLowerInvariant() == queryNameLower)
                    {
                        matchesToCreate.Add(new DomainMatchModel
                        {
                            KnownDomainId = knownDomain.Id,
                            DnsPacketId = packet.Id,
                            MatchType = DomainMatchType.Exact
                        });
                        foundExactMatch = true;
                    }
                }

                if (foundExactMatch)
                {
                    continue;
                }

                foreach (var knownDomain in knownDomains)
                {
                    var knownDomainLower = knownDomain.DomainName.ToLowerInvariant();
                    if (queryNameLower.EndsWith("." + knownDomainLower) &&
                        queryNameLower.Length > knownDomainLower.Length + 1)
                    {
                        matchesToCreate.Add(new DomainMatchModel
                        {
                            KnownDomainId = knownDomain.Id,
                            DnsPacketId = packet.Id,
                            MatchType = DomainMatchType.Subdomain
                        });
                    }
                }
            }

            if (matchesToCreate.Count != 0)
            {
                await domainMatchFacade.CreateMatchesAsync(matchesToCreate);
            }
        }
    }
}
