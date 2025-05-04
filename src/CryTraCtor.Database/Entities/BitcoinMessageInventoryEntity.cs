using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[PrimaryKey(nameof(BitcoinMessageId), nameof(BitcoinInventoryId))]
public class BitcoinMessageInventoryEntity
{
    public Guid BitcoinMessageId { get; set; }
    public BitcoinMessageEntity BitcoinMessage { get; set; } = null!;
    public Guid BitcoinInventoryId { get; set; }
    public BitcoinInventoryEntity BitcoinInventory { get; set; } = null!;
}
