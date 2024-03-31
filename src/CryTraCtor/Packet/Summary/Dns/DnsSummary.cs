namespace CryTraCtor.Packet.Summary.Dns;

public abstract record DnsSummary(
    string SourceAddress,
    string DestinationAddress,
    int SourcePort,
    int DestinationPort,
    DnsMessageType MessageType,
    uint TransactionId
) : PacketSummary(SourceAddress, DestinationAddress, SourcePort, DestinationPort), IDnsSummary
{
    public override string GetSerializedPacketString()
    {
        return SourceAddress + ":" + SourcePort + " -> " + DestinationAddress + ":" + DestinationPort +
               Environment.NewLine + "TxId: " + TransactionId + " Type: " + MessageType
            ;
    }
}