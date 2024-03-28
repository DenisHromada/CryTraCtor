using System.Collections.ObjectModel;

namespace CryTraCtor.PacketParsers.Summary.Dns;

public record DnsResponse(
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort,
    DnsMessageType MessageType,
    uint TransactionId,
    Collection<QueryEntry> Queries,
    Collection<AnswerEntry> Answers
) : DnsQuery(SourceAddress, DestinationAddress, SourcePort, DestinationPort, MessageType, TransactionId, Queries);