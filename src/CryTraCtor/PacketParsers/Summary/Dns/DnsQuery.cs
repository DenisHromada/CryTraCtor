using System.Collections.ObjectModel;

namespace CryTraCtor.PacketParsers.Summary.Dns;

public record DnsQuery(
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort,
    DnsMessageType MessageType,
    uint TransactionId,
    Collection<QueryEntry> Queries
) : DnsSummary(SourceAddress, DestinationAddress, SourcePort, DestinationPort, MessageType, TransactionId);