using CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Bitcoin;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.Packet.Services;

public class BitcoinPacketReader
{

    public IEnumerable<IBitcoinPacketSummary> Read(string fileName)
    {
        using ICaptureDevice device = new CaptureFileReaderDevice(fileName);
        device.Open();
        device.Filter = "(ip or ip6) and tcp and port 8333";

        while (device.GetNextPacket(out var packetCapture) == GetPacketStatus.PacketRead)
        {
            yield return PacketCaptureMapper.MapToBitcoinPacketSummary(packetCapture);
        }

        device.Close();
    }
}
