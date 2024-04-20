using System.Collections.ObjectModel;

namespace CryTraCtor.DataTypes.Packet.Summary.Dns;

public record DnsResponse(
    InternetEndpoint Source,
    InternetEndpoint Destination,
    uint TransactionId,
    DnsResourceRecord Query,
    Collection<DnsResourceRecord> Answers
) : DnsSummary(Source, Destination, DnsMessageType.Response, TransactionId)
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Query) + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Answers);
    }
}