using CryTraCtor.Packet.Summary.Dns;
using SharpPcap;

namespace CryTraCtor.PacketParsers.RawToSummaryMapper.Dns;

public static class DnsMapper
{
    public static IDnsSummary MapDnsPacketToDnsSummary(PacketCapture packetCapture)
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

        return dnsPacket.GetDnsMessageType() switch
        {
            DnsMessageType.Query => new DnsQuery(
                ipPacket.SourceAddress.ToString(),
                ipPacket.DestinationAddress.ToString(),
                udpPacket.SourcePort,
                udpPacket.DestinationPort,
                dnsPacket.GetTransactionId(),
                dnsPacket.GetQueries()
            ),
            DnsMessageType.Response => new DnsResponse(
                ipPacket.SourceAddress.ToString(),
                ipPacket.DestinationAddress.ToString(),
                udpPacket.SourcePort,
                udpPacket.DestinationPort,
                dnsPacket.GetTransactionId(),
                dnsPacket.GetQueries(),
                dnsPacket.GetAnswers()
            ),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}