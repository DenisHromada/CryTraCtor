using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using CryTraCtor.Packet.Models;
using PacketDotNet;
using SharpPcap;

namespace CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Bitcoin;

public static class PacketCaptureMapper
{
    public static IBitcoinPacketSummary MapToBitcoinPacketSummary(PacketCapture packetCapture)
    {
        var rawPacket = packetCapture.GetPacket();
        var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

        var ipPacket = packet.Extract<IPPacket>();
        var tcpPacket = packet.Extract<TcpPacket>();

        if (ipPacket == null || tcpPacket == null)
        {
            throw new InvalidOperationException(
                "Packet could not be parsed as a valid IP/TCP packet for Bitcoin summary.");
        }

        var source = new InternetEndpointModel(ipPacket.SourceAddress.ToString(), tcpPacket.SourcePort);
        var destination = new InternetEndpointModel(ipPacket.DestinationAddress.ToString(), tcpPacket.DestinationPort);
        var timestamp = rawPacket.Timeval.Date;

        return new BitcoinPacketSummary(timestamp, source, destination);
    }
}
