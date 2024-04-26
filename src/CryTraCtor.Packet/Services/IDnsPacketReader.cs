using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;

namespace CryTraCtor.Packet.Services;

public interface IDnsPacketReader
{
    IEnumerable<IDnsPacketSummary> Read(string fileName);
    
}