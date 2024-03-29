namespace CryTraCtor.PacketParsers.Summary.Dns;

public interface IDnsSummary : IPacketSummary
{
    public DnsMessageType MessageType { get; }
    public uint TransactionId { get; }
}