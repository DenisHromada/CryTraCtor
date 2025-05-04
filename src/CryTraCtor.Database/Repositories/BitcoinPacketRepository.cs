using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class BitcoinPacketRepository(CryTraCtorDbContext dbContext, IEntityMapper<BitcoinPacketEntity> entityMapper)
    : Repository<BitcoinPacketEntity>(dbContext, entityMapper), IBitcoinPacketRepository
{
    public async Task<IEnumerable<BitcoinPacketEntity>> GetByInventoryIdAndAnalysisIdAsync(Guid inventoryId, Guid fileAnalysisId)
    {
        return await Get()
            .Include(p => p.Sender)
            .Include(p => p.Recipient)
            .Where(p => p.FileAnalysisId == fileAnalysisId && p.BitcoinPacketInventories.Any(bpi => bpi.BitcoinInventoryId == inventoryId))
            .ToListAsync();
    }
}
