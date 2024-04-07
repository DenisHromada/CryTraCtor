namespace CryTraCtor.Models.Packet.Summary.Dns;

public interface IDnsSummary : IPacketSummary
{
    public DnsMessageType MessageType { get; }
    public uint TransactionId { get; }
}