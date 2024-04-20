namespace CryTraCtor.DataTypes.Packet.Summary.Dns;

public record DnsQuery(
    InternetEndpoint Source,
    InternetEndpoint Destination,
    uint TransactionId,
    DnsResourceRecord Query
) : DnsSummary(Source, Destination, DnsMessageType.Query, TransactionId)
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Query);
    }
}