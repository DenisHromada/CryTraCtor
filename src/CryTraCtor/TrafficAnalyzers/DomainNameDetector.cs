using CryTraCtor.PacketParsers.RawToSummaryMapper.Dns;
using CryTraCtor.PacketParsers.Summary.Dns;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.TrafficAnalyzers;

public class DomainNameDetector(string analyzedFileName) : TrafficAnalyzer(analyzedFileName)
{
    private static int _dnsPacketCounter = 0;

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
        var dnsSummary = DnsMapper.MapDnsPacketToDnsSummary(packetCapture);

        _dnsPacketCounter++;
        Console.WriteLine("DNS packet #{0}", _dnsPacketCounter);

        if (dnsSummary.MessageType == DnsMessageType.Query)
        {
            var dnsQuery = (DnsQuery)dnsSummary;

            Console.WriteLine(dnsQuery.GetSerializedPacketString());
        }
        else
        {
            var dnsResponse = (DnsResponse)dnsSummary;

            Console.WriteLine(dnsResponse.GetSerializedPacketString());
        }


        // Console.WriteLine(dnsSummary.GetSerializedPacketString());
    }
}