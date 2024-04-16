using System.Collections.ObjectModel;
using CryTraCtor.Models.Packet.Summary.Dns;

namespace CryTraCtor.Models.DnsTransaction;

public record DnsTransactionTraffic(Collection<DnsResponse> Responses, Collection<DnsQuery> Queries)
{
    public Collection<DnsQuery> Queries { get; } = [];
    public Collection<DnsResponse> Responses { get; } = [];

    public void AddQuery(DnsQuery query)
    {
        Queries.Add(query);
    }

    public void AddResponse(DnsResponse response)
    {
        Responses.Add(response);
    }

    public void AddDnsSummary(IDnsSummary dnsSummary)
    {
        if (dnsSummary.MessageType == DnsMessageType.Query)
        {
            AddQuery((DnsQuery)dnsSummary);
        }
        else
        {
            AddResponse((DnsResponse)dnsSummary);
        }
    }
}