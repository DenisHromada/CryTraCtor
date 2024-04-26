using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

public abstract record DnsPacketSummary(
    InternetEndpointModel Source,
    InternetEndpointModel Destination,
    DnsMessageType MessageType,
    uint TransactionId
) : PacketSummary(Source, Destination), IDnsPacketSummary
{
    public override string GetSerializedPacketString()
    {
        return Source + " -> " + Destination +
               Environment.NewLine + "TxId: " + TransactionId + " Type: " + MessageType
            ;
    }
}