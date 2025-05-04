using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[PrimaryKey(nameof(BitcoinMessageId), nameof(BitcoinTransactionId))]
public class BitcoinMessageTransactionEntity
{
    public Guid BitcoinMessageId { get; set; }
    public BitcoinMessageEntity BitcoinMessage { get; set; } = null!;

    public Guid BitcoinTransactionId { get; set; }
    public BitcoinTransactionEntity BitcoinTransaction { get; set; } = null!;
}
