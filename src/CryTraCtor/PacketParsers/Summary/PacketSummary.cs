namespace CryTraCtor.PacketParsers.Summary;

public abstract record PacketSummary(
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort
);