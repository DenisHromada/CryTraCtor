using System.Collections.ObjectModel;
using CryTraCtor.Models;
using CryTraCtor.Packet.Summary.Dns;

namespace CryTraCtor.Analyzers;

public class KnownDomainDetector(Dictionary<uint, Collection<IDnsSummary>> dnsTransactions)
{
    public Dictionary<uint, KnownDomainDetail> KnownDomainDetails { get; } = new();
    public void Run()
    {
        foreach (var dnsTransaction in dnsTransactions)
        {
            // search transaction at a time and add relevant information for every known domain
            var dnsTransactionValue = dnsTransaction.Value;

            foreach (var dnsSummary in dnsTransactionValue)
            {
                if (dnsSummary.MessageType == DnsMessageType.Query)
                {
                    var dnsQuery = (DnsQuery)dnsSummary;
                    foreach (var query in dnsQuery.Queries)
                    {
                        var knownDomainDetail = IsDomainKnown(query.Name);
                        if (knownDomainDetail != null)
                        {
                            Console.WriteLine(knownDomainDetail);
                        }
                    }
                }
                else
                {
                    var dnsResponse = (DnsResponse)dnsSummary;
                    foreach (var query in dnsResponse.Answers)
                    {
                        var knownDomainDetail = IsDomainKnown(query.Name);
                        if (knownDomainDetail != null)
                        {
                            Console.WriteLine(knownDomainDetail);
                        }
                    }
                }
            }
        }
    }

    private static KnownDomainDetail? IsDomainKnown(string domainName)
    {
        if (domainName.EndsWith("trezor.io"))
        {
            return new KnownDomainDetail(
                domainName,
                "Trezor",
                "Trezor Suite",
                "Main domain",
                "Any",
                "No description"
            );
        }

        return null;
    }
}