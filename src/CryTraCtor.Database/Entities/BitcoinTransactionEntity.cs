using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(TxId), IsUnique = true)]
public class BitcoinTransactionEntity : IEntity
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string TxId { get; set; } = string.Empty;

    public int Version { get; set; }
    public uint Locktime { get; set; }

    public ICollection<BitcoinTransactionInputEntity> Inputs { get; set; } = new HashSet<BitcoinTransactionInputEntity>();
    public ICollection<BitcoinTransactionOutputEntity> Outputs { get; set; } = new HashSet<BitcoinTransactionOutputEntity>();
    public ICollection<BitcoinPacketTransactionEntity> BitcoinPacketTransactions { get; set; } = new HashSet<BitcoinPacketTransactionEntity>();
}
