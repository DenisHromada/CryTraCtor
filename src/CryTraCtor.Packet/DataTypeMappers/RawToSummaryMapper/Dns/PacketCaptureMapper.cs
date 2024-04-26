using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Models;
using SharpPcap;

namespace CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Dns;

public static class PacketCaptureMapper
{
    public static IDnsPacketSummary MapToDnsPacketSummary(PacketCapture packetCapture)
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

        var source = new InternetEndpointModel(
            ipPacket.SourceAddress.ToString(),
            udpPacket.SourcePort
        );
        
        var destination = new InternetEndpointModel(
            ipPacket.DestinationAddress.ToString(),
            udpPacket.DestinationPort
        );


        return dnsPacket.GetDnsMessageType() switch
        {
            DnsMessageType.Query => new DnsPacketQuery(
                source,
                destination,
                dnsPacket.GetTransactionId(),
                dnsPacket.GetQuery()
            ),
            DnsMessageType.Response => new DnsPacketResponse(
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