using System.Collections.ObjectModel;

namespace CryTraCtor.PacketParsers.Summary.Dns;

public record DnsQuery(
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort,
    uint TransactionId,
    Collection<QueryEntry> Queries
) : DnsSummary(SourceAddress, DestinationAddress, SourcePort, DestinationPort, DnsMessageType.Query, TransactionId)
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Queries);
    }
}