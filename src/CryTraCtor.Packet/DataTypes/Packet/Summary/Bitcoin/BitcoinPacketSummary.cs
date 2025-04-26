using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;

public record BitcoinPacketSummary(
    DateTime Timestamp,
    InternetEndpointModel Source,
    InternetEndpointModel Destination
) : PacketSummary(Timestamp, Source, Destination), IBitcoinPacketSummary
{
}
