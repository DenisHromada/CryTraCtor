using System.Net;

namespace CryTraCtor.Packet.Models;

public record IpEndpointModel(IPAddress IpAddress, ushort Port);
