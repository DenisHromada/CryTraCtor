using SharpPcap;

namespace CryTraCtor.PacketArrivalHandler;

public static class PacketArrivalHandler
{
    public static void HandlePacketArrival(object s, PacketCapture packetCapture)
    {
        var rawPacket = packetCapture.GetPacket();
        var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

        var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
        if (tcpPacket == null)
        {
            return;
        }

        var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
        var srcIp = ipPacket.SourceAddress;
        var dstIp = ipPacket.DestinationAddress;
        int srcPort = tcpPacket.SourcePort;
        int dstPort = tcpPacket.DestinationPort;

        Console.WriteLine("{0}:{1} -> {2}:{3}",
            srcIp, srcPort, dstIp, dstPort);
    }
}