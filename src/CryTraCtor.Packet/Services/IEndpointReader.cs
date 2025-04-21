using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.Services;

public interface IEndpointReader
{
    IEnumerable<IpEndpointModel> GetEndpoints(string pcapFilePath);
}
