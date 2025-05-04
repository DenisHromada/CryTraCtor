using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[PrimaryKey(nameof(BitcoinPacketId), nameof(BitcoinTransactionId))]
public class BitcoinPacketTransactionEntity
{
    public Guid BitcoinPacketId { get; set; }
    public BitcoinMessageEntity BitcoinMessage { get; set; } = null!;

    public Guid BitcoinTransactionId { get; set; }
    public BitcoinTransactionEntity BitcoinTransaction { get; set; } = null!;
}
