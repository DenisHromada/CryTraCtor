using CryTraCtor.Models;
using CryTraCtor.Models.Packet.Summary.Dns;
using SharpPcap;

namespace CryTraCtor.Mappers.RawToSummaryMapper.Dns;

public static class DnsMapper
{
    public static IDnsSummary MapPacketCaptureToDnsPacketSummary(PacketCapture packetCapture)
    {
        var rawPacket = packetCapture.GetPacket();
        var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

        var udpPacket = packet.Extract<PacketDotNet.UdpPacket>();
        if (udpPacket == null)
        {
            throw new InvalidOperationException("Packet is not a UDP packet");
        }

        var ipPacket = (PacketDotNet.IPPacket)udpPacket.ParentPacket;

        var dnsPacket = new DnsPayloadWrapper(udpPacket.PayloadData);

        var source = new InternetEndpoint(
            ipPacket.SourceAddress.ToString(),
            udpPacket.SourcePort
        );
        
        var destination = new InternetEndpoint(
            ipPacket.DestinationAddress.ToString(),
            udpPacket.DestinationPort
        );


        return dnsPacket.GetDnsMessageType() switch
        {
            DnsMessageType.Query => new DnsQuery(
                source,
                destination,
                dnsPacket.GetTransactionId(),
                dnsPacket.GetQuery()
            ),
            DnsMessageType.Response => new DnsResponse(
                source,
                destination,
                dnsPacket.GetTransactionId(),
                dnsPacket.GetQuery(),
                dnsPacket.GetAnswers()
            ),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}