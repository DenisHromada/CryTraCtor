namespace CryTraCtor.PacketParsers.Summary.Dns;

public abstract record DnsSummary (
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort,
    DnsMessageType MessageType,
    uint TransactionId
) : PacketSummary(SourceAddress, DestinationAddress, SourcePort, DestinationPort), IDnsSummary;