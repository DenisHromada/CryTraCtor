using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary;

public record BasicPacketSummary(
    DateTime Timestamp,
    InternetEndpointModel Source,
    InternetEndpointModel Destination
) : PacketSummary(Timestamp, Source, Destination);
