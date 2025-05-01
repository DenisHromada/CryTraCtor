using System.ComponentModel.DataAnnotations.Schema;

namespace CryTraCtor.Database.Entities;

[Table("BitcoinPacket")]
public class BitcoinPacketEntity : PacketEntityBase
{
    public uint Magic { get; set; }
    public string Command { get; set; } = string.Empty;
    public uint Length { get; set; }
    public uint Checksum { get; set; }
}
