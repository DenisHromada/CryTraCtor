using CryTraCtor.Packet.Models;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.Packet.Services;

public class EndpointReader : IEndpointReader
{
    public IEnumerable<IpEndpointModel> GetEndpoints(string pcapFilePath)
    {
        var endpoints = new HashSet<IpEndpointModel>();
        var device = new CaptureFileReaderDevice(pcapFilePath);

        device.Open();
        device.OnPacketArrival += (sender, e) => HandlePacket(e.GetPacket(), endpoints);
        device.Capture();
        device.Close();

        return endpoints;
    }

    private static void HandlePacket(RawCapture rawCapture, HashSet<IpEndpointModel> endpoints)
    {
        var packet = PacketDotNet.Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
        var ipPacket = packet.Extract<IPPacket>();

        if (ipPacket == null)
        {
            return;
        }

        var sourceIp = ipPacket.SourceAddress;
        var destinationIp = ipPacket.DestinationAddress;

        ushort sourcePort = 0;
        ushort destinationPort = 0;

        var tcpPacket = ipPacket.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            sourcePort = tcpPacket.SourcePort;
            destinationPort = tcpPacket.DestinationPort;
        }
        else
        {
            var udpPacket = ipPacket.Extract<UdpPacket>();
            if (udpPacket != null)
            {
                sourcePort = udpPacket.SourcePort;
                destinationPort = udpPacket.DestinationPort;
            }
        }

        if (sourcePort != 0)
        {
            endpoints.Add(new IpEndpointModel(sourceIp, sourcePort));
        }
        if (destinationPort != 0)
        {
            endpoints.Add(new IpEndpointModel(destinationIp, destinationPort));
        }
    }
}
