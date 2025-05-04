using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[PrimaryKey(nameof(BitcoinMessageId), nameof(BitcoinBlockHeaderId))]
public class BitcoinMessageHeaderEntity
{
    public Guid BitcoinMessageId { get; set; }
    public BitcoinMessageEntity BitcoinMessage { get; set; } = null!;

    public Guid BitcoinBlockHeaderId { get; set; }
    public BitcoinBlockHeaderEntity BitcoinBlockHeader { get; set; } = null!;
}
