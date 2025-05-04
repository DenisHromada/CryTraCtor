using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(Type), nameof(Hash), IsUnique = true)]
public class BitcoinInventoryEntity : IEntity
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(64)]
    public string Hash { get; set; } = string.Empty;

    public ICollection<BitcoinPacketInventoryEntity> BitcoinPacketInventories { get; set; } = new HashSet<BitcoinPacketInventoryEntity>();
}
