using CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Dns;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.Packet.Services;

public class DnsPacketReader
{
    public IEnumerable<IDnsPacketSummary> Read(string fileName)
    {
        using ICaptureDevice device = new CaptureFileReaderDevice(fileName);
        device.Open();
        device.Filter = "(ip or ip6) and udp and port 53";

        while (device.GetNextPacket(out var packetCapture) == GetPacketStatus.PacketRead)
        {
            yield return PacketCaptureMapper.MapToDnsPacketSummary(packetCapture);
        }

        device.Close();
    }
}
