using SharpPcap;

namespace CryTraCtor.PacketArrivalHandler;

public static class PacketArrivalHandler
{
    public static void HandlePacketArrival(object s, PacketCapture packet)
    {
        Console.WriteLine(packet.GetPacket());
    }
}
