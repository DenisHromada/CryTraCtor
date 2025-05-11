using CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Generic;
using CryTraCtor.Packet.DataTypes.Packet.Summary;
using SharpPcap;
using SharpPcap.LibPcap;

namespace CryTraCtor.Packet.Services;

public class GenericPacketReaderService
{
    private const string DefaultIpFilter = "(ip or ip6) and (tcp or udp)";

    public IEnumerable<IPacketSummary> ReadIpPackets(string pcapFilePath, string? bpfFilter = null)
    {
        if (string.IsNullOrEmpty(pcapFilePath))
        {
            yield break;
        }


        ICaptureDevice device;
        try
        {
            device = new CaptureFileReaderDevice(pcapFilePath);
            device.Open();
        }
        catch (PcapException ex)
        {
            throw new FileNotFoundException($"PCAP file error: {ex.Message}", pcapFilePath, ex);
        }

        using (device)
        {
            device.Filter = !string.IsNullOrWhiteSpace(bpfFilter)
                ? bpfFilter
                : DefaultIpFilter;


            PacketCapture packetCapture;
            while ((device.GetNextPacket(out packetCapture)) == GetPacketStatus.PacketRead)
            {
                var summary = GenericPacketCaptureMapper.MapToGenericPacketSummary(packetCapture);
                if (summary != null)
                {
                    yield return summary;
                }
            }
        }
    }
}
