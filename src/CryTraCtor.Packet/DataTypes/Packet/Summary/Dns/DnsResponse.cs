using System.Collections.ObjectModel;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

public record DnsResponse(
    InternetEndpointModel Source,
    InternetEndpointModel Destination,
    uint TransactionId,
    DnsResourceRecordModel Query,
    Collection<DnsResourceRecordModel> Answers
) : DnsSummary(Source, Destination, DnsMessageType.Response, TransactionId)
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Query) + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Answers);
    }
}