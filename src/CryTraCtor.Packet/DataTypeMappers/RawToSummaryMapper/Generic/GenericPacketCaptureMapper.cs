using CryTraCtor.Packet.DataTypes.Packet.Summary;
using CryTraCtor.Packet.Models;
using PacketDotNet;
using SharpPcap;

namespace CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Generic;

public static class GenericPacketCaptureMapper
{
    public static IPacketSummary? MapToGenericPacketSummary(PacketCapture packetCapture)
    {
        var rawPacket = packetCapture.GetPacket();
        if (rawPacket == null)
        {
            return null;
        }

        var parsedPacket = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

        var ipPacket = parsedPacket?.Extract<IPPacket>();
        if (ipPacket == null)
        {
            return null;
        }

        var transportPacket = ipPacket.Extract<TcpPacket>() ?? (TransportPacket?)ipPacket.Extract<UdpPacket>();

        if (transportPacket == null)
        {
            return null;
        }

        var sourceAddress = ipPacket.SourceAddress.ToString();
        var destinationAddress = ipPacket.DestinationAddress.ToString();

        if (string.IsNullOrEmpty(sourceAddress) || string.IsNullOrEmpty(destinationAddress))
        {
            return null;
        }

        var sourceEndpoint = new InternetEndpointModel(sourceAddress, transportPacket.SourcePort);
        var destinationEndpoint = new InternetEndpointModel(destinationAddress, transportPacket.DestinationPort);
        var timestamp = rawPacket.Timeval.Date;

        return new BasicPacketSummary(timestamp, sourceEndpoint, destinationEndpoint);
    }
}
