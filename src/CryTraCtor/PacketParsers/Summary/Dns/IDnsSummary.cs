namespace CryTraCtor.PacketParsers.Summary.Dns;

public interface IDnsSummary
{
    public uint TransactionId { get; }
    public DnsMessageType MessageType { get; }
}