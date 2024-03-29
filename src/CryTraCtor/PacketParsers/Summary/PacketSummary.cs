namespace CryTraCtor.PacketParsers.Summary;

public abstract record PacketSummary(
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort
) : IPacketSummary
{
    public virtual string GetSerializedPacketString()
    {
        return SourceAddress + ":" + SourcePort + " -> " + DestinationAddress + ":" + DestinationPort;
    }
}