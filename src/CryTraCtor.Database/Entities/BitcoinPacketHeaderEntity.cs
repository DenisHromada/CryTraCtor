using System;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[PrimaryKey(nameof(BitcoinPacketId), nameof(BitcoinBlockHeaderId))]
public class BitcoinPacketHeaderEntity
{
    public Guid BitcoinPacketId { get; set; }
    public BitcoinMessageEntity BitcoinMessage { get; set; } = null!;

    public Guid BitcoinBlockHeaderId { get; set; }
    public BitcoinBlockHeaderEntity BitcoinBlockHeader { get; set; } = null!;
}
