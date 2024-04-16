namespace CryTraCtor.Models.Packet.Summary;

public interface IPacketSummary
{
    public InternetEndpoint Source { get; }
    public InternetEndpoint Destination { get; }

    public string GetSerializedPacketString();
}