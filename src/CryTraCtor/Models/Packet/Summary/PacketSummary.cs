namespace CryTraCtor.Models.Packet.Summary;

public abstract record PacketSummary(
    InternetEndpoint Source,
    InternetEndpoint Destination
) : IPacketSummary
{
    public virtual string GetSerializedPacketString()
    {
        return Source + " -> " + Destination;
    }
}