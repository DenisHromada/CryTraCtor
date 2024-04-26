using System.Collections.ObjectModel;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

namespace CryTraCtor.Packet.DataTypes.DnsTransaction;

public record DnsTransactionTraffic(Collection<DnsPacketResponse> Responses, Collection<DnsPacketQuery> Queries)
{
    public Collection<DnsPacketQuery> Queries { get; } = [];
    public Collection<DnsPacketResponse> Responses { get; } = [];

    public void AddQuery(DnsPacketQuery packetQuery)
    {
        Queries.Add(packetQuery);
    }

    public void AddResponse(DnsPacketResponse packetResponse)
    {
        Responses.Add(packetResponse);
    }

    public void AddDnsSummary(IDnsPacketSummary dnsPacketSummary)
    {
        if (dnsPacketSummary.MessageType == DnsMessageType.Query)
        {
            AddQuery((DnsPacketQuery)dnsPacketSummary);
        }
        else
        {
            AddResponse((DnsPacketResponse)dnsPacketSummary);
        }
    }
}