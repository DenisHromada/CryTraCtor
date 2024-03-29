using System.Collections.ObjectModel;

namespace CryTraCtor.PacketParsers.Summary.Dns;

public record DnsResponse(
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort,
    uint TransactionId,
    Collection<QueryEntry> Queries,
    Collection<AnswerEntry> Answers
) : DnsSummary(SourceAddress, DestinationAddress, SourcePort, DestinationPort, DnsMessageType.Response, TransactionId)
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Queries) + Environment.NewLine
                                                + string.Join("," + Environment.NewLine, Answers);
    }
}