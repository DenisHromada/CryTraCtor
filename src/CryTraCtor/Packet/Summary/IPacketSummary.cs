namespace CryTraCtor.Packet.Summary;

public interface IPacketSummary
{
    public string SourceAddress { get; }
    public string DestinationAddress { get; }
    public int SourcePort { get; }
    public int DestinationPort { get; }

    public string GetSerializedPacketString();
}