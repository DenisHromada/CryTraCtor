using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Table("BitcoinBlockHeader")]
[Index(nameof(BlockHash), IsUnique = true)]
public class BitcoinBlockHeaderEntity : IEntity
{
    public Guid Id { get; set; }

    public int Version { get; set; }

    [MaxLength(64)] public string BlockHash { get; set; } = string.Empty;

    [MaxLength(64)] public string PrevBlockHash { get; set; } = string.Empty;

    [MaxLength(64)] public string MerkleRoot { get; set; } = string.Empty;

    public DateTimeOffset Timestamp { get; set; }

    public uint Bits { get; set; }

    public uint Nonce { get; set; }

    public ICollection<BitcoinPacketHeaderEntity> BitcoinPacketHeaders { get; set; } =
        new HashSet<BitcoinPacketHeaderEntity>();
}
