namespace CryTraCtor.Packet.Models;

public record InternetEndpointModel(string Address, int Port)
{
    private string Address { get; set; } = Address;
    private int Port { get; set; } = Port;
    
    public override string ToString()
    {
        return Address + ":" + Port;
    }
}