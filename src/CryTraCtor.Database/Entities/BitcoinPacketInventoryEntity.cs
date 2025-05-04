using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[PrimaryKey(nameof(BitcoinPacketId), nameof(BitcoinInventoryId))]
public class BitcoinPacketInventoryEntity
{
    public Guid BitcoinPacketId { get; set; }
    public BitcoinMessageEntity BitcoinMessage { get; set; } = null!;
    public Guid BitcoinInventoryId { get; set; }
    public BitcoinInventoryEntity BitcoinInventory { get; set; } = null!;
}
