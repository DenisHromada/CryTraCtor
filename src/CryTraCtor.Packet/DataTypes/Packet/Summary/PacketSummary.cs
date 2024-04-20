using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary;

public abstract record PacketSummary(
    InternetEndpointModel Source,
    InternetEndpointModel Destination
) : IPacketSummary
{
    public virtual string GetSerializedPacketString()
    {
        return Source + " -> " + Destination;
    }
}