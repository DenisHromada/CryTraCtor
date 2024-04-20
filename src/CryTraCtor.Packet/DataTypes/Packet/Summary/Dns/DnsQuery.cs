using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

public record DnsQuery(
    InternetEndpointModel Source,
    InternetEndpointModel Destination,
    uint TransactionId,
    DnsResourceRecordModel Query
) : DnsSummary(Source, Destination, DnsMessageType.Query, TransactionId)
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Query);
    }
}