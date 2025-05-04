using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryTraCtor.Database.Entities;

public class BitcoinTransactionInputEntity : IEntity
{
    public Guid Id { get; set; }

    public Guid BitcoinTransactionId { get; set; }

    [ForeignKey(nameof(BitcoinTransactionId))]
    public BitcoinTransactionEntity BitcoinTransaction { get; set; } = null!;

    [MaxLength(64)] public string PreviousTxHash { get; set; } = string.Empty;
    public uint PreviousOutputIndex { get; set; }

    public string ScriptSig { get; set; } = string.Empty;
    public uint Sequence { get; set; }
}
