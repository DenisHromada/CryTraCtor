using System.Collections.ObjectModel;
using CryTraCtor.Models;
using CryTraCtor.Models.DnsTransaction;
using Microsoft.VisualBasic;

namespace CryTraCtor.Analyzers;

public class KnownDomainDetector(Collection<DnsTransactionSummary> dnsTransactions)
{
    public Dictionary<KnownDomainDetail, Collection<DnsTransactionSummary>> KnownDomainDetails { get; } = new();
    public Collection<DnsTransactionSummary> FilteredDnsTransactions = [];

    public void Run()
    {
        foreach (var dnsTransactionSummary in dnsTransactions)
        {
            if (IsDomainKnown(dnsTransactionSummary.Query.Name))
            {
                FilteredDnsTransactions.Add(dnsTransactionSummary);
            }
        }
    }
    public void OldRun()
    {
        foreach (var dnsTransaction in dnsTransactions)
        {
            var domainName = dnsTransaction.Query.Name;
            // search transaction at a time and add relevant information for every known domain
            if (IsDomainKnown(domainName))
            {
                AddDomainDetail(GetKnownDomainDetail(domainName), dnsTransaction);
            }
        }
    }

    private static bool IsDomainKnown(string domainName)
    {
        return domainName.EndsWith("trezor.io");
    }

    public static KnownDomainDetail? GetKnownDomainDetail(string domainName)
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

    private void AddDomainDetail(KnownDomainDetail? key, DnsTransactionSummary value)
    {
        if (key == null)
        {
            return;
        }
        if (KnownDomainDetails.TryGetValue(key, out var valueCollection))
        {
            valueCollection.Add(value);
        }
        else
        {
            KnownDomainDetails.Add(key, [value]);
        }
    }
    
    public Dictionary<KnownDomainDetail, Collection<string>> GetKnownDomainIpAddresses()
    {
        var knownDomainIpAddresses = new Dictionary<KnownDomainDetail, Collection<string>>();
        
        foreach (var knownDomainKeyPair in KnownDomainDetails)
        {
            knownDomainIpAddresses.Add(knownDomainKeyPair.Key, []);
            foreach (var dnsTransaction in knownDomainKeyPair.Value)
            {
                foreach (var record in dnsTransaction.Answers)
                {
                    if (record.RecordType == "A")
                    {
                        knownDomainIpAddresses[knownDomainKeyPair.Key].Add(record.RecordData);
                    }
                }
            }
        }

        return knownDomainIpAddresses;
    }
}