using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

public record DnsPacketQuery(
    InternetEndpointModel Source,
    InternetEndpointModel Destination,
    DateTime Timestamp,
    uint TransactionId,
    DnsResourceRecordModel Query
) : DnsPacketSummary(Source, Destination, Timestamp, DnsMessageType.Query, TransactionId)
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Query);
    }
}
