using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.TrafficAnalyzers;

public class DomainNameDetector(string analyzedFileName) : TrafficAnalyzer(analyzedFileName)
{
    public override void Run()
    {
        using var device = new CaptureFileReaderDevice(AnalyzedFileName);

        device.Open();
        device.Filter = "ip and tcp";

        device.OnPacketArrival += HandlePacketArrival;
        
        device.Capture();

    }
    
    private static void HandlePacketArrival(object s, PacketCapture packetCapture)
    {
        var rawPacket = packetCapture.GetPacket();
        var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
        
        var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
        if (tcpPacket == null) {return;}
        
        var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
        var srcIp = ipPacket.SourceAddress;
        var dstIp = ipPacket.DestinationAddress;
        int srcPort = tcpPacket.SourcePort;
        int dstPort = tcpPacket.DestinationPort;

        Console.WriteLine("{0}:{1} -> {2}:{3}",
            srcIp, srcPort, dstIp, dstPort);
    }
}