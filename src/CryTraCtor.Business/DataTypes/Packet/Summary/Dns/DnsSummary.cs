namespace CryTraCtor.DataTypes.Packet.Summary.Dns;

public abstract record DnsSummary(
    InternetEndpoint Source,
    InternetEndpoint Destination,
    DnsMessageType MessageType,
    uint TransactionId
) : PacketSummary(Source, Destination), IDnsSummary
{
    public override string GetSerializedPacketString()
    {
        return Source + " -> " + Destination +
               Environment.NewLine + "TxId: " + TransactionId + " Type: " + MessageType
            ;
    }
}