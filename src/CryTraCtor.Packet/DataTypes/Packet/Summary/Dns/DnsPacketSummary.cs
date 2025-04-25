using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

public abstract record DnsPacketSummary(
    InternetEndpointModel Source,
    InternetEndpointModel Destination,
    DateTime Timestamp,
    DnsMessageType MessageType,
    uint TransactionId
) : PacketSummary(Timestamp, Source, Destination), IDnsPacketSummary
{
    public override string GetSerializedPacketString()
    {
        return base.GetSerializedPacketString() +
               Environment.NewLine + "TxId: " + TransactionId + " Type: " + MessageType
            ;
    }
}
