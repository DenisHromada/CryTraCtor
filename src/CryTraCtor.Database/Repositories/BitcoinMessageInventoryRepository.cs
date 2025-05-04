using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Repositories.Interfaces;

namespace CryTraCtor.Database.Repositories;

public class BitcoinMessageInventoryRepository(CryTraCtorDbContext dbContext) : IBitcoinMessageInventoryRepository
{
    public IQueryable<BitcoinMessageInventoryEntity> Get() => dbContext.Set<BitcoinMessageInventoryEntity>();

    public async Task<BitcoinMessageInventoryEntity> InsertAsync(BitcoinMessageInventoryEntity entity)
    {
        await dbContext.Set<BitcoinMessageInventoryEntity>().AddAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(Guid bitcoinMessageId, Guid bitcoinInventoryId)
    {
        var entity = await dbContext.Set<BitcoinMessageInventoryEntity>().FindAsync(bitcoinMessageId, bitcoinInventoryId);
        if (entity != null)
        {
            dbContext.Set<BitcoinMessageInventoryEntity>().Remove(entity);
        }
    }
}
