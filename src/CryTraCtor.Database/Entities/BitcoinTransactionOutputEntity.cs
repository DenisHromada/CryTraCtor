using System.ComponentModel.DataAnnotations.Schema;

namespace CryTraCtor.Database.Entities;

public class BitcoinTransactionOutputEntity : IEntity
{
    public Guid Id { get; set; }

    [ForeignKey(nameof(BitcoinTransactionId))]
    public Guid BitcoinTransactionId { get; set; }

    public BitcoinTransactionEntity BitcoinTransaction { get; set; } = null!;

    public long Value { get; set; }
    public string ScriptPubKey { get; set; } = string.Empty;
}
