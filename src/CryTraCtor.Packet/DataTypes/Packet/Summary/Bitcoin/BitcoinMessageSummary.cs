using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin
{
    public class BitcoinMessageSummary : IBitcoinPacketSummary
    {
        public DateTime Timestamp { get; set; }
        public InternetEndpointModel Source { get; set; }
        public InternetEndpointModel Destination { get; set; }
        public string Command { get; set; }
        public uint PayloadSize { get; set; }
        public uint Magic { get; set; }
        public uint Checksum { get; set; }

        public string GetSerializedPacketString()
        {
            return $"Command: {Command}, PayloadSize: {PayloadSize}, Magic: 0x{Magic:X8}, Checksum: 0x{Checksum:X8}";
        }
    }
}
