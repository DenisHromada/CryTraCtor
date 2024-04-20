namespace CryTraCtor.DataTypes.Packet.Summary.Dns;

public interface IDnsSummary : IPacketSummary
{
    public DnsMessageType MessageType { get; }
    public uint TransactionId { get; }
}