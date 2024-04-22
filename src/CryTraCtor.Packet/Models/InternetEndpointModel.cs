using System.Runtime.Serialization;

namespace CryTraCtor.Packet.Models;

public record InternetEndpointModel(string Address, int Port)
{
    public string Address { get; set; } = Address;
    public int Port { get; set; } = Port;
    
    public override string ToString()
    {
        return Address + ":" + Port;
    }
}