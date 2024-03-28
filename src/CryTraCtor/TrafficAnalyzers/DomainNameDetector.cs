using System.Collections.ObjectModel;
using CryTraCtor.PacketParsers.RawToSummaryMapper.Dns;
using Kaitai;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.TrafficAnalyzers;

public class DomainNameDetector(string analyzedFileName) : TrafficAnalyzer(analyzedFileName)
{
    private static int DnsPacketCounter = 0;
    
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
        var dnsPacket = new DnsPacket(dataStream);
        
        DnsPacketCounter++;
        Console.WriteLine("DNS packet #{0}", DnsPacketCounter);
        Console.WriteLine("Transaction ID: {0:X}", dnsPacket.TransactionId);
        
        foreach (var query in dnsPacket.GetQueries())
        {
            Console.WriteLine("Query: {0}", query.DomainName);
        }
        
        foreach (var answer in dnsPacket.GetAnswers())
        {
            Console.WriteLine("Answer: {0}", answer.DomainName);
        }
        
        var srcIp = ipPacket.SourceAddress;
        var dstIp = ipPacket.DestinationAddress;
        int srcPort = udpPacket.SourcePort;
        int dstPort = udpPacket.DestinationPort;

        Console.WriteLine("{0}:{1} -> {2}:{3}",
            srcIp, srcPort, dstIp, dstPort);
    }
}