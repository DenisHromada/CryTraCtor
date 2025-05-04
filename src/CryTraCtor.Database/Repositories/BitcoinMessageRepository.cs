using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Repositories;

public class BitcoinMessageRepository(CryTraCtorDbContext dbContext, IEntityMapper<BitcoinMessageEntity> entityMapper)
    : Repository<BitcoinMessageEntity>(dbContext, entityMapper), IBitcoinMessageRepository
{
    public async Task<IEnumerable<BitcoinMessageEntity>> GetByInventoryIdAndAnalysisIdAsync(Guid inventoryId, Guid fileAnalysisId)
    {
        return await Get()
            .Include(p => p.Sender)
            .Include(p => p.Recipient)
            .Where(p => p.FileAnalysisId == fileAnalysisId && p.BitcoinMessageInventories.Any(bpi => bpi.BitcoinInventoryId == inventoryId))
            .ToListAsync();
    }
}
