namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

public interface IDnsPacketSummary : IPacketSummary
{
    public DateTime Timestamp { get; }
    public DnsMessageType MessageType { get; }
    public uint TransactionId { get; }
}
