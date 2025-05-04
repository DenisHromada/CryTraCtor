using System.ComponentModel.DataAnnotations.Schema;

namespace CryTraCtor.Database.Entities;

[Table("BitcoinPacket")]
public class BitcoinMessageEntity : PacketEntityBase
{
    public uint Magic { get; set; }
    public string Command { get; set; } = string.Empty;
    public uint Length { get; set; }
    public uint Checksum { get; set; }

    public ICollection<BitcoinPacketInventoryEntity> BitcoinPacketInventories { get; set; } = new HashSet<BitcoinPacketInventoryEntity>();
    public ICollection<BitcoinPacketTransactionEntity> BitcoinPacketTransactions { get; set; } = new HashSet<BitcoinPacketTransactionEntity>();
    public ICollection<BitcoinPacketHeaderEntity> BitcoinPacketHeaders { get; set; } = new HashSet<BitcoinPacketHeaderEntity>();
}
