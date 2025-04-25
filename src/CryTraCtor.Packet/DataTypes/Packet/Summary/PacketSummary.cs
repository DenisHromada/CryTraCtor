using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary;

public abstract record PacketSummary(
    DateTime Timestamp,
    InternetEndpointModel Source,
    InternetEndpointModel Destination
) : IPacketSummary
{
    public virtual string GetSerializedPacketString()
    {
        return Timestamp + " : " + Source + " -> " + Destination;
    }
}
