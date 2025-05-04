namespace CryTraCtor.Database.Entities;

public class BitcoinMessageEntity : MessageEntityBase
{
    public uint Magic { get; set; }
    public string Command { get; set; } = string.Empty;
    public uint Length { get; set; }
    public uint Checksum { get; set; }

    public ICollection<BitcoinMessageInventoryEntity> BitcoinMessageInventories { get; set; } = new HashSet<BitcoinMessageInventoryEntity>();
    public ICollection<BitcoinMessageTransactionEntity> BitcoinMessageTransactions { get; set; } = new HashSet<BitcoinMessageTransactionEntity>();
    public ICollection<BitcoinMessageHeaderEntity> BitcoinMessageHeaders { get; set; } = new HashSet<BitcoinMessageHeaderEntity>();
}
