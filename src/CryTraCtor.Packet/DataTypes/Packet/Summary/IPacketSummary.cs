using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary;

public interface IPacketSummary
{
    public DateTime Timestamp { get; }
    public InternetEndpointModel Source { get; }
    public InternetEndpointModel Destination { get; }

    public string GetSerializedPacketString();
}
