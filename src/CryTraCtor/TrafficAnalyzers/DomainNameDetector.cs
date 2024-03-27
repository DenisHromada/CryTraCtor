using CryTraCtor.DataStructures;
using Kaitai;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.TrafficAnalyzers;

public class DomainNameDetector(string analyzedFileName) : TrafficAnalyzer(analyzedFileName)
{
    protected static int DnsPacketCounter = 0;
    public override void Run()
    {
        using var device = new CaptureFileReaderDevice(AnalyzedFileName);

        device.Open();
        device.Filter = "ip and udp and port 53";

        device.OnPacketArrival += HandlePacketArrival;
        
        device.Capture();

    }
    
    private static void HandlePacketArrival(object s, PacketCapture packetCapture)
    {
        var rawPacket = packetCapture.GetPacket();
        var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
        
        var udpPacket = packet.Extract<PacketDotNet.UdpPacket>();
        if (udpPacket == null) {return;}

        var ipPacket = (PacketDotNet.IPPacket)udpPacket.ParentPacket;
        
        var dataStream = new KaitaiStream(udpPacket.PayloadData);
        var data = new DnsPacket(dataStream);
        DnsPacketCounter++;
        Console.WriteLine("DNS packet #{0}", DnsPacketCounter);
        Console.WriteLine("Transaction ID: {0:X}", data.TransactionId);
        
        foreach (var query in data.Queries)
        {
            var domainName = data.GetFullyQualifiedDomainName(query.Name);
            
            Console.WriteLine("Query: {0}", domainName.TrimEnd('.'));
        }
        
        foreach (var answer in data.Answers)
        {
            var domainName = data.GetFullyQualifiedDomainName(answer.Name);
            
            Console.WriteLine("Answer: {0}", domainName.TrimEnd('.'));
        }
        
        // var protocol = udpPayloadPacket.
        var srcIp = ipPacket.SourceAddress;
        var dstIp = ipPacket.DestinationAddress;
        int srcPort = udpPacket.SourcePort;
        int dstPort = udpPacket.DestinationPort;

        Console.WriteLine("{0}:{1} -> {2}:{3}",
            srcIp, srcPort, dstIp, dstPort);
    }
}